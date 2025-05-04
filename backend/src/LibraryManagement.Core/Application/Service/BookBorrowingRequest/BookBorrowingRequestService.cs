using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Application.Interface.Services;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Application.Service
{
    public class BookBorrowingRequestService : IBookBorrowingRequestService
    {
        private const int MaxRequestsPerMonth = 3;
        private const int MaxBooksPerRequest = 5;

        private readonly IBookBorrowingRequestRepo _requestRepo;
        private readonly IBookBorrowingRequestDetailsRepo _detailsRepo;
        private readonly IBookService _bookService;
        private readonly IUnitOfWork _unitOfWork;

        public BookBorrowingRequestService(
            IBookBorrowingRequestRepo requestRepo,
            IBookBorrowingRequestDetailsRepo detailsRepo,
            IBookService bookService,
            IUserRepo userRepo,
            IUnitOfWork unitOfWork
        )
        {
            _requestRepo = requestRepo;
            _detailsRepo = detailsRepo;
            _bookService = bookService;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> CreateBorrowRequestAsync(BorrowRequest request)
        {
            if (request.BookIds == null || !request.BookIds.Any())
                return OperationResult.Fail("No books specified for borrowing.");

            if (request.BookIds.Count > MaxBooksPerRequest)
                return OperationResult.Fail(
                    $"A request can contain at most {MaxBooksPerRequest} books."
                );

            var countThisMonth = await _requestRepo.GetNumOfRequestThisMonth(request.RequestorID);
            if (countThisMonth >= MaxRequestsPerMonth)
                return OperationResult.Fail(
                    $"Request limit reached ({MaxRequestsPerMonth} per month)."
                );

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // create request header
                var header = new BookBorrowingRequest
                {
                    Id = Guid.NewGuid(),
                    RequestorId = request.RequestorID,
                    DateRequested = DateTime.UtcNow,
                    Status = RequestStatus.Waiting,
                };
                await _requestRepo.AddAsync(header);

                var detailsList = new List<BookBorrowingRequestDetails>();
                foreach (var bookId in request.BookIds.Distinct())
                {
                    // decrease availability
                    var ok = await _bookService.DecreaseAvailableAmount(bookId, 1);
                    if (!ok)
                    {
                        await _unitOfWork.RollbackAsync();
                        return OperationResult.Fail($"Book with ID {bookId} is not available.");
                    }
                    // build detail
                    detailsList.Add(
                        new BookBorrowingRequestDetails
                        {
                            Id = Guid.NewGuid(),
                            BookId = bookId,
                            RequestId = header.Id,
                            Status = BorrowBookStatus.Borrowed,
                        }
                    );
                }

                await _detailsRepo.AddRangeAsync(detailsList);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                // prepare response
                var response = await BuildBorrowResponseAsync(header.Id);
                return OperationResult.Ok(response, "Borrow request created.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail($"Failed to create borrow request: {ex.Message}");
            }
        }

        public async Task<OperationResult> GetBorrowRequestByIdAsync(Guid requestId)
        {
            var response = await BuildBorrowResponseAsync(requestId);
            if (response == null)
                return OperationResult.Fail("Request not found.");
            return OperationResult.Ok(response, "Request retrieved.");
        }

        public async Task<OperationResult> GetRequestsByUserAsync(Guid requestorId)
        {
            var entities = await _requestRepo.GetRequestsWithDetailsByUserAsync(requestorId);
            var responses = new List<BorrowResponse>();
            foreach (var ent in entities)
            {
                var resp = await BuildBorrowResponseAsync(ent.Id);
                if (resp != null)
                    responses.Add(resp);
            }
            return OperationResult.Ok(responses, "User requests retrieved.");
        }

        private async Task<BorrowResponse?> BuildBorrowResponseAsync(Guid requestId)
        {
            var allForUser = await _requestRepo.GetBorrowResponsesByRequestIdAsync(requestId);

            var response = allForUser.FirstOrDefault(r => r.Id == requestId);
            return response;
        }

        public async Task<OperationResult> ReturnBooksAsync(ReturnRequest request)
        {
            // 1) Fetch request with details
            var header = await _requestRepo.GetByIdAsync(request.RequestId);
            if (header == null)
                return OperationResult.Fail("Borrow request not found.");

            // 2) Filter the details that match the return list
            var returnDetails = header
                .BookBorrowingRequestDetails.Where(d => request.BookIds.Contains(d.BookId))
                .ToList();

            if (!returnDetails.Any())
                return OperationResult.Fail("No matching borrowed books found to return.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 3) Process each return
                foreach (var detail in returnDetails)
                {
                    if (detail.Status != BorrowBookStatus.Borrowed)
                        continue; // or fail if you want strictness

                    // mark as returned
                    detail.Status = BorrowBookStatus.Returned;
                    _detailsRepo.Update(detail);

                    // increase availability
                    await _bookService.IncreaseAvailableAmount(detail.BookId, 1);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                // 5) Return updated DTO
                var response = await BuildBorrowResponseAsync(request.RequestId);
                return OperationResult.Ok(response!, "Books returned successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail($"Failed to return books: {ex.Message}");
            }
        }

        // In BookBorrowingRequestService
        public async Task<OperationResult> SetApprovalAsync(
            Guid requestId,
            Guid approverId,
            bool isApproved
        )
        {
            var header = await _requestRepo.GetByIdAsync(requestId);
            if (header == null)
                return OperationResult.Fail("Request not found.");

            if (header.Status != RequestStatus.Waiting)
                return OperationResult.Fail("Only waiting requests can be processed.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                header.ApproverId = approverId;
                header.Status = isApproved ? RequestStatus.Approved : RequestStatus.Rejected;
                _requestRepo.Update(header);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                var response = await BuildBorrowResponseAsync(requestId);
                return OperationResult.Ok(
                    response!,
                    isApproved ? "Request approved." : "Request declined."
                );
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail($"Failed to process request: {ex.Message}");
            }
        }
    }
}
