using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public record ResetPasswordRequest
{
    [Required]
    public required string Token { get; set; }

    [Required, MinLength(8)]
    public required string Password { get; set; }

    [Required, Compare(nameof(Password))]
    public required string ConfirmPassword { get; set; }
}
