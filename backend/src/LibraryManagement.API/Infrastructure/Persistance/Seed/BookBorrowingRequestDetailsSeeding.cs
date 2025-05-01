using LibraryManagement.Core.Application.Service.Security;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.API.Infrastructure.Persistence.Seed
{
    public static class BookBorrowingRequestDetailsSeeding
    {
        public static List<BookBorrowingRequestDetails> SeedBookRequestDetails()
        {
            var bookRequestDetails = new List<BookBorrowingRequestDetails>
            {
                new BookBorrowingRequestDetails
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    BookId = Guid.Parse("33333333-3333-3333-3333-333333333333"), // Book: "Introduction to Algorithms"
                    RequestId = Guid.Parse("77777777-7777-7777-7777-777777777777"), // Request: BorrowingRequest
                },
            };

            return bookRequestDetails;
        }
    }
}
