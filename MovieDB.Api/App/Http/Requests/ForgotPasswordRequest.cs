using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public class ForgotPasswordRequest
{
    [Required, EmailAddress]
    public required string Email { get; set; }
}
