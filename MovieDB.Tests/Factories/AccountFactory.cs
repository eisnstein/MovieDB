using System;
using MovieDB.Api.Entities;

namespace MovieDB.Tests.Factories
{
    public class AccountFactory
    {
        public static Account CreateAccount()
        {
            return new Account
            {
                Id = 1,
                Title = null,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("secret"),
                AcceptTerms = true,
                Role = Role.Admin,
                VerificationToken = null,
                VerifiedAt = null,
                ResetToken = null,
                ResetTokenExpiresAt = null,
                PasswordResetAt = null,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                UpdatedAt = null,
                RefreshTokens = null
            };
        }
    }
}
