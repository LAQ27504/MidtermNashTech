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
                    BookId = Guid.Parse("33333333-3333-3333-3333-333333333333"), // "Introduction to Algorithms"
                    RequestId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Status = BorrowBookStatus.Approved,
                },
                new BookBorrowingRequestDetails
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    BookId = Guid.Parse("55555555-5555-5555-5555-555555555555"), // "Clean Code"
                    RequestId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Status = BorrowBookStatus.Approved,
                },
                new BookBorrowingRequestDetails
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    BookId = Guid.Parse("77777777-7777-7777-7777-777777777777"), // "The Pragmatic Programmer"
                    RequestId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Status = BorrowBookStatus.Returned,
                },
                new BookBorrowingRequestDetails
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    BookId = Guid.Parse("44444444-4444-4444-4444-444444444444"), // "Hamlet"
                    RequestId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Status = BorrowBookStatus.Waiting,
                },
                new BookBorrowingRequestDetails
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    BookId = Guid.Parse("66666666-6666-6666-6666-666666666666"), // "To Kill a Mockingbird"
                    RequestId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Status = BorrowBookStatus.Waiting,
                },
            };

            return bookRequestDetails;
        }
    }
}
