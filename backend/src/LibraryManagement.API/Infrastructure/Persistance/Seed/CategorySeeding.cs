using LibraryManagement.Core.Application.Service.Security;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.API.Infrastructure.Persistence.Seed
{
    public static class CategorySeeding
    {
        public static List<Category> SeedCategory()
        {
            var bookRequestDetails = new List<Category>
            {
                new Category
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
