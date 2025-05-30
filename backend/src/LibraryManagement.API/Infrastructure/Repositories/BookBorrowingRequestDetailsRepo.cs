using LibraryManagement.API.Infrastructure.Persistence.DBContext;
using LibraryManagement.API.Infrastructure.Repositories;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Core.Infrastructure.Repositories
{
    public class BookBorrowingRequestDetailsRepo
        : BaseRepo<BookBorrowingRequestDetails>,
            IBookBorrowingRequestDetailsRepo
    {
        public BookBorrowingRequestDetailsRepo(ApplicationDBContext context)
            : base(context) { }

        public async Task<ICollection<BookBorrowingRequestDetails>> GetRequestDetailByRequestId(
            Guid requestId
        )
        {
            var requestDetails = await _context
                .BooksBorrowingRequestDetails.Where(d => d.RequestId == requestId)
                .Include(d => d.BorrowBook)
                .ToListAsync();

            if (requestDetails == null)
            {
                throw new Exception("Request details not found");
            }

            return requestDetails;
        }

        public async Task UpdateRequestDetailStatusAsync(Guid requestId, BorrowBookStatus status)
        {
            var requestDetails = await _context
                .BooksBorrowingRequestDetails.Where(d => d.RequestId == requestId)
                .ToListAsync();

            if (requestDetails == null)
            {
                throw new Exception("Request details not found");
            }

            foreach (var detail in requestDetails)
            {
                detail.Status = status;
            }

            await _context.SaveChangesAsync();

            return;
        }
    }
}
