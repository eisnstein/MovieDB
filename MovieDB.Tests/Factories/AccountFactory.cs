using MovieDB.Api.App.Models;

namespace MovieDB.Tests.Factories;

public static class AccountFactory
{
    public static Account CreateAccount(int id = 1, string email = "john@test.local", bool verified = false)
        => new Account
        {
            Id = id,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            Role = Role.Admin,
            VerificationToken = null,
            VerifiedAt = verified ? DateTime.UtcNow.AddDays(-7) : null,
            ResetToken = null,
            ResetTokenExpiresAt = null,
            PasswordResetAt = null,
            CreatedAt = DateTime.UtcNow.AddDays(-7),
            UpdatedAt = null,
            RefreshTokens = null
        };
}
