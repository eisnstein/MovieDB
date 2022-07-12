using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public class RevokeTokenRequest
{
    [Required]
    public string? Token { get; set; }
}
