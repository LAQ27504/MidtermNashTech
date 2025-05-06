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
                    RequestorId = Guid.Parse("55555555-5555-5555-5555-555555555555"), // user1
                    ApproverId = Guid.Parse("66666666-6666-6666-6666-666666666666"), // admin
                },
                new BookBorrowingRequest
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    DateRequested = new DateTime(2025, 4, 30),
                    Status = RequestStatus.Waiting,
                    RequestorId = Guid.Parse("99999999-9999-9999-9999-999999999999"), // user3
                    ApproverId = null,
                },
                new BookBorrowingRequest
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    DateRequested = new DateTime(2025, 4, 25),
                    Status = RequestStatus.Rejected,
                    RequestorId = Guid.Parse("88888888-8888-8888-8888-888888888888"), // user2
                    ApproverId = Guid.Parse("66666666-6666-6666-6666-666666666666"), // admin
                },
                new BookBorrowingRequest
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    DateRequested = new DateTime(2025, 4, 26),
                    Status = RequestStatus.Approved,
                    RequestorId = Guid.Parse("55555555-5555-5555-5555-555555555555"), // user1
                    ApproverId = Guid.Parse("77777777-7777-7777-7777-777777777777"), // supervisor
                },
                new BookBorrowingRequest
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    DateRequested = new DateTime(2025, 4, 29),
                    Status = RequestStatus.Waiting,
                    RequestorId = Guid.Parse("99999999-9999-9999-9999-999999999999"), // user3
                    ApproverId = null,
                },
            };

            return bookRequests;
        }
    }
}
