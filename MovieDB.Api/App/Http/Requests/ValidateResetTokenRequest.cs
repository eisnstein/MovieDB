using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public class ValidateResetTokenRequest
{
    [Required]
    public string? Token { get; set; }
}
