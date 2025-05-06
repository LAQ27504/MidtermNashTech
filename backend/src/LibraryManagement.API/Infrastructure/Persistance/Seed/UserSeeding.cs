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
                new User
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Name = "supervisor",
                    HashedPassword = "supervisor123",
                    Type = UserType.SuperUser,
                },
                new User
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "user2",
                    HashedPassword = "password2",
                    Type = UserType.NormalUser,
                },
                new User
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Name = "user3",
                    HashedPassword = "password3",
                    Type = UserType.NormalUser,
                },
                new User
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "manager",
                    HashedPassword = "manager123",
                    Type = UserType.SuperUser,
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
