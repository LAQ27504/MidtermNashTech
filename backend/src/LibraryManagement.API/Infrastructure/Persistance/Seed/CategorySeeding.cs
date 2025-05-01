using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Infrastructure.Persistance.Seed
{
    public static class CategorySeeding
    {
        public static List<Category> SeedCategories()
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Computer Science",
                },
                new Category
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Literature",
                },
            };

            return categories;
        }
    }
}
