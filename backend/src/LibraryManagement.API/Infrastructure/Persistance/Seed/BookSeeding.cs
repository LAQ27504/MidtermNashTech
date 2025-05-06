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
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Computer Science
                    Amount = 10,
                    AvailableAmount = 10,
                },
                new Book
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Hamlet",
                    Author = "William Shakespeare",
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Literature
                    Amount = 5,
                    AvailableAmount = 5,
                },
                new Book
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Clean Code",
                    Author = "Robert C. Martin",
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Computer Science
                    Amount = 7,
                    AvailableAmount = 7,
                },
                new Book
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "1984",
                    Author = "George Orwell",
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Literature
                    Amount = 8,
                    AvailableAmount = 8,
                },
                new Book
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Name = "The Pragmatic Programmer",
                    Author = "Andrew Hunt",
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Computer Science
                    Amount = 6,
                    AvailableAmount = 6,
                },
                new Book
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Literature
                    Amount = 5,
                    AvailableAmount = 5,
                },
                new Book
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Name = "Design Patterns",
                    Author = "Erich Gamma",
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Computer Science
                    Amount = 4,
                    AvailableAmount = 4,
                },
                new Book
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Pride and Prejudice",
                    Author = "Jane Austen",
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Literature
                    Amount = 6,
                    AvailableAmount = 6,
                },
                new Book
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Name = "Operating System Concepts",
                    Author = "Abraham Silberschatz",
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Computer Science
                    Amount = 5,
                    AvailableAmount = 5,
                },
                new Book
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Name = "Macbeth",
                    Author = "William Shakespeare",
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Literature
                    Amount = 3,
                    AvailableAmount = 3,
                },
            };

            return books;
        }
    }
}
