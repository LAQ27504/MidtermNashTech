using LibraryManagement.Core.Application.Service.Security;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.API.Infrastructure.Persistence.Seed
{
    public static class BookSeeding
    {
        public static List<Book> SeedBooks()
        {
            var books = new List<Book>
            {
                new Book
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Introduction to Algorithms",
                    Author = "Thomas H. Cormen",
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Assuming you map Category manually since your CategoryId is int
                    Amount = 10,
                    AvailableAmount = 10,
                },
                new Book
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Hamlet",
                    Author = "William Shakespeare",
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Amount = 5,
                    AvailableAmount = 5,
                },
            };

            return books;
        }
    }
}
