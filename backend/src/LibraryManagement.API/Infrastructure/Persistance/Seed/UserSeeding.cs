using LibraryManagement.Core.Application.Service.Security;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.API.Infrastructure.Persistence.Seed
{
    public static class UserSeeding
    {
        public static List<User> SeedUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "admin",
                    HashedPassword = "admin123",
                    Type = UserType.SuperUser,
                },
                new User
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "user1",
                    HashedPassword = "password1",
                    Type = UserType.NormalUser,
                },
            };

            foreach (var user in users)
            {
                user.HashedPassword = PasswordService.HashPassword(user, user.HashedPassword);
            }

            return users;
        }
    }
}
