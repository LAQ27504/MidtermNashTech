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
        private readonly TokenInfoService _tokenInfoService;

        public BookBorrowingRequestService(
            IBookBorrowingRequestRepo requestRepo,
            IBookBorrowingRequestDetailsRepo detailsRepo,
            IBookService bookService,
            IUnitOfWork unitOfWork,
            TokenInfoService tokenInfoService
        )
        {
            _requestRepo = requestRepo;
            _detailsRepo = detailsRepo;
            _bookService = bookService;
            _unitOfWork = unitOfWork;
            _tokenInfoService = tokenInfoService;
        }

        public async Task<OperationResult> CreateBorrowRequestAsync(BorrowRequest request)
        {
            var (userId, userName, userRole) = _tokenInfoService.GetTokenInfo();

            if (request.RequestorID != userId)
            {
                return OperationResult.Fail("Requestor ID does not match the logged-in user.");
            }

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
                            Status = BorrowBookStatus.Waiting,
                        }
                    );
                }

                await _detailsRepo.AddRangeAsync(detailsList);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                // prepare response
                var response = await BuildBorrowResponseAsync(header.Id);
                return OperationResult.Ok(response!, "Borrow request created.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail($"Failed to create borrow request: {ex.Message}");
            }
        }

        public async Task<OperationResult> GetAllRequestsAsync()
        {
            var entities = await _requestRepo.GetAllRequestsWithDetailsAsync();
            var responses = new List<BorrowResponse>();
            foreach (var ent in entities)
            {
                var resp = await BuildBorrowResponseAsync(ent.Id);
                if (resp != null)
                    responses.Add(resp);
            }

            return OperationResult.Ok(responses, "All requests retrieved.");
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

        public async Task<OperationResult> GetRequestWithPaginationUserIdAsync(
            int pageNumber,
            int pageSize,
            Guid requestorId
        )
        {
            if (pageNumber < 1 || pageSize < 1)
                return OperationResult.Fail("Page number and size must be greater than zero.");

            var (entities, total) = await _requestRepo.GetPagedAsync(pageNumber, pageSize);
            var filteredEntities = entities.Where(r => r.RequestorId == requestorId).ToList();
            var totalFiltered = filteredEntities.Count;
            if (totalFiltered == 0)
                return OperationResult.Fail("No requests found for the user.");
            var responses = new List<BorrowResponse>();
            foreach (var ent in filteredEntities)
            {
                var resp = await BuildBorrowResponseAsync(ent.Id);
                if (resp != null)
                    responses.Add(resp);
            }
            var pagedResponse = new PaginationResponse<BorrowResponse>
            {
                Items = responses,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return OperationResult.Ok(pagedResponse, "Requests retrieved with pagination.");
        }

        public async Task<OperationResult> GetRequestWithPaginationWaitingAsync(
            int pageNumber,
            int pageSize
        )
        {
            if (pageNumber < 1 || pageSize < 1)
                return OperationResult.Fail("Page number and size must be greater than zero.");

            var (entities, total) = await _requestRepo.GetPagedAsync(pageNumber, pageSize);

            var filteredEntities = entities.Where(r => r.Status == RequestStatus.Waiting).ToList();

            var totalFiltered = filteredEntities.Count;
            if (totalFiltered == 0)
                return OperationResult.Ok("No requests found for the user.");
            var responses = new List<BorrowResponse>();
            foreach (var ent in filteredEntities)
            {
                var resp = await BuildBorrowResponseAsync(ent.Id);
                if (resp != null)
                    responses.Add(resp);
            }
            var pagedResponse = new PaginationResponse<BorrowResponse>
            {
                Items = responses,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return OperationResult.Ok(pagedResponse, "Requests retrieved with pagination.");
        }

        public async Task<OperationResult> ReturnBooksAsync(ReturnRequest request)
        {
            var (userId, userName, userRole) = _tokenInfoService.GetTokenInfo();

            var header = await _requestRepo.GetByIdAsync(request.RequestId);
            if (header == null)
                return OperationResult.Fail("Borrow request not found.");

            if (header.RequestorId != userId)
                return OperationResult.Fail("Requestor ID does not match the logged-in user.");

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
                    if (detail.Status != BorrowBookStatus.Approved)
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
        public async Task<OperationResult> SetApprovalAsync(Guid requestId, bool isApproved)
        {
            var (userId, userName, userRole) = _tokenInfoService.GetTokenInfo();
            var requestApprove = await _requestRepo.GetByIdAsync(requestId);
            if (requestApprove == null)
                return OperationResult.Fail("Request not found.");

            if (requestApprove.Status != RequestStatus.Waiting)
                return OperationResult.Fail("Only waiting requests can be processed.");
            if (requestApprove.ApproverId != null)
                return OperationResult.Fail("Request already processed.");
            // Check if the approver is a librarian
            if (userRole != nameof(UserType.SuperUser))
                return OperationResult.Fail("Only Super User can approve requests.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                requestApprove.ApproverId = userId;
                requestApprove.Status = isApproved
                    ? RequestStatus.Approved
                    : RequestStatus.Rejected;

                await _detailsRepo.UpdateRequestDetailStatusAsync(
                    requestId,
                    isApproved ? BorrowBookStatus.Approved : BorrowBookStatus.Rejected
                );

                _requestRepo.Update(requestApprove);

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
