using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public record RegisterRequest
{
    [Required, EmailAddress]
    public string? Email { get; init; }

    [Required, MinLength(8)]
    public string? Password { get; init; }

    [Required, Compare(nameof(Password))]
    public string? ConfirmPassword { get; init; }
}
