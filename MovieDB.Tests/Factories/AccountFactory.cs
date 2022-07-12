using MovieDB.Api.App.Models;

namespace MovieDB.Tests.Factories;

public static class AccountFactory
{
    public static Account CreateAccount()
        => new Account
        {
            Id = 1,
            Email = "john@test.local",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
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
