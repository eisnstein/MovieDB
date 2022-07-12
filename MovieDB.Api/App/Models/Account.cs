namespace MovieDB.Api.App.Models;

public class Account
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public Role Role { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public bool IsVerified => VerifiedAt.HasValue || PasswordResetAt.HasValue;
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiresAt { get; set; }
    public DateTime? PasswordResetAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<RefreshToken>? RefreshTokens { get; set; }

    public bool OwnsToken(string token)
    {
        return this.RefreshTokens?.Find(t => t.Token == token) is not null;
    }
}
