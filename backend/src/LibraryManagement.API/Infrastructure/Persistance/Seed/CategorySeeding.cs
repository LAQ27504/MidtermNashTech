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
                new Category
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "History",
                },
                new Category
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Mathematics",
                },
                new Category
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Science",
                },
                new Category
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "Art",
                },
                new Category
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Name = "Philosophy",
                },
                new Category
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "Engineering",
                },
                new Category
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Name = "Medicine",
                },
                new Category
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Business",
                },
            };

            return categories;
        }
    }
}
