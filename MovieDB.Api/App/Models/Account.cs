using MovieDB.Api.App.Http.Responses;

namespace MovieDB.Api.App.Models;

public class Account
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public Role Role { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public bool IsVerified => VerifiedAt.HasValue || PasswordResetAt.HasValue;
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiresAt { get; set; }
    public DateTime? PasswordResetAt { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<RefreshToken>? RefreshTokens { get; set; }

    public bool OwnsToken(string token)
    {
        return this.RefreshTokens?.Find(t => t.Token == token) is not null;
    }

    public AccountResponse ToResponse()
    {
        return new AccountResponse()
        {
            Id = Id,
            Email = Email,
            Role = Role.ToString(),
            IsVerified = IsVerified,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}
