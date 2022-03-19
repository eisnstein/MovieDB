using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MovieDB.Api.Entities;

[Owned]
public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    public Account Account { get; set; } = default!;
    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime CreatedAt { get; set; }
    public string CreatedByIp { get; set; } = default!;
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }
    public bool IsActive => RevokedAt == null && !IsExpired;
}
