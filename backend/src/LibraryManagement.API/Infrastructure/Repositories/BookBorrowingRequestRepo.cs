using LibraryManagement.API.Infrastructure.Persistence.DBContext;
using LibraryManagement.API.Infrastructure.Repositories;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Core.Infrastructure.Repositories
{
    public class BookBorrowingRequestRepo
        : BaseRepo<BookBorrowingRequest>,
            IBookBorrowingRequestRepo
    {
        public BookBorrowingRequestRepo(ApplicationDBContext context)
            : base(context) { }

        public async Task<ICollection<BookBorrowingRequest>> GetWaitingRequests()
        {
            return await _context
                .BookBorrowingRequests.Where(request => request.Status == RequestStatus.Waiting)
                .ToListAsync();
        }

        public async Task<List<BookBorrowingRequest>> GetRequestsWithDetailsByUserAsync(
            Guid requestorId
        )
        {
            return await _context
                .BookBorrowingRequests.Where(r => r.RequestorId == requestorId)
                .Include(r => r.BookBorrowingRequestDetails)
                .ThenInclude(d => d.BorrowBook)
                .ToListAsync();
        }

        public async Task<int> GetNumOfRequestThisMonth(Guid requestorId)
        {
            var now = DateTime.UtcNow;
            return await _context.BookBorrowingRequests.CountAsync(r =>
                r.RequestorId == requestorId
                && r.DateRequested.Month == now.Month
                && r.DateRequested.Year == now.Year
            );
        }

        public async Task<ICollection<BorrowResponse>> GetBorrowResponsesByRequestIdAsync(
            Guid requestId
        )
        {
            // 1) Query the requests with details and book navigations
            var query = _context
                .BookBorrowingRequests.AsNoTracking()
                .Where(r => r.Id == requestId)
                .Include(r => r.BookBorrowingRequestDetails)
                .ThenInclude(d => d.BorrowBook)
                .OrderByDescending(r => r.DateRequested);

            // 2) Project into your DTO
            var result = await query
                .Select(r => new BorrowResponse
                {
                    Id = r.Id,
                    RequestorID = r.RequestorId.ToString(),
                    RequestorName = r.Requestor.Name, // make sure you Include Requestor or load it separately
                    BorrowDate = r.DateRequested,
                    ApproverID = r.ApproverId.HasValue
                        ? r.ApproverId.Value.ToString()
                        : string.Empty,
                    ApproverName = r.Approver!.Name ?? string.Empty,
                    Status = r.Status,
                    Details = r
                        .BookBorrowingRequestDetails.Select(
                            d => new BookBorrowingRequestDetailsResponse
                            {
                                Id = d.Id,
                                BookId = d.BookId,
                                BookName = d.BorrowBook.Name,
                                BookAuthor = d.BorrowBook.Author,
                                Status = d.Status,
                            }
                        )
                        .ToList(),
                })
                .ToListAsync();

            return result;
        }
    }
}
