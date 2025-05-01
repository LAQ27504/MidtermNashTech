using LibraryManagement.Core.Application.Service.Security;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.Extensions.ObjectPool;

namespace LibraryManagement.Core;

public static class Class1
{
    public static void main()
    {
        var user = new User
        {
            Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            Name = "Jane Smith",
            Type = UserType.SuperUser,
            HashedPassword = "hashed_password_2",
        };
        var pass = PasswordService.HashPassword(user, "1234");

        Console.WriteLine(pass);

        user.HashedPassword = pass;

        var verify = PasswordService.VerifyPassword(user, "1234");

        Console.WriteLine(verify);
    }
}
