using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public class VerifyEmailRequest
{
    [Required]
    public string? Token { get; set; }
}
