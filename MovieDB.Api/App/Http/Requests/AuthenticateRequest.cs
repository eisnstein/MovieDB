using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public class AuthenticateRequest
{
    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
