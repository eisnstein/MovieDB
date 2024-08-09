using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public record RegisterRequest
{
    [Required, EmailAddress]
    public required string Email { get; init; }

    [Required, MinLength(8)]
    public required string Password { get; init; }

    [Required, Compare(nameof(Password))]
    public required string ConfirmPassword { get; init; }
}
