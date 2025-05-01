using LibraryManagement.Core.Application.Service.Security;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.API.Infrastructure.Persistence.Seed
{
    public static class BookBorrowingRequestSeeding
    {
        public static List<BookBorrowingRequest> SeedBookRequests()
        {
            var bookRequests = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    DateRequested = new DateTime(2025, 4, 28),
                    Status = RequestStatus.Approved,
                    RequestorId = Guid.Parse("55555555-5555-5555-5555-555555555555"), // Again, assuming mapping manually if needed
                    ApproverId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                },
            };

            return bookRequests;
        }
    }
}
