using LibraryManagement.Core.Domains.Entities;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Core.Application.Security
{
    public static class PasswordService
    {
        private static readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public static string HashPassword(User user, string plainPassword)
        {
            return _passwordHasher.HashPassword(user, plainPassword);
        }

        public static bool VerifyPassword(User user, string plainPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.HashedPassword,
                plainPassword
            );
            return result == PasswordVerificationResult.Success;
        }
    }
}
