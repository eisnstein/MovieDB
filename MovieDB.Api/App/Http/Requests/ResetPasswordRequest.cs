using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public record ResetPasswordRequest
{
    [Required]
    public string? Token { get; set; }

    [Required, MinLength(8)]
    public string? Password { get; set; }

    [Required, Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }
}
