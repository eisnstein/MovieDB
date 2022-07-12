using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public class ForgotPasswordRequest
{
    [Required, EmailAddress]
    public string? Email { get; set; }
}
