using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public class ValidateResetTokenRequest
{
    [Required]
    public required string Token { get; set; }
}
