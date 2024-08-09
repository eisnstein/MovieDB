using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public record AccountUpdateRequest
{
    [EmailAddress]
    public string? Email;

    [MinLength(8)]
    public string? Password;

    public string? ConfirmPassword;

    public string? Role;
}
