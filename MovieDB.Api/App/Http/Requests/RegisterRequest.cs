using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public record RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; init; } = default!;

    [Required, MinLength(8)]
    public string Password { get; init; } = default!;

    [Required, Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = default!;
}
