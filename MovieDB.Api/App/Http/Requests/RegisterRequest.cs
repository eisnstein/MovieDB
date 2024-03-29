using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Required, MinLength(8)]
    public string Password { get; set; } = default!;

    [Required, Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = default!;
}
