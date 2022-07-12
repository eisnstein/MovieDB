using System.Text.Json.Serialization;

namespace MovieDB.Api.App.Http.Responses;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string Role { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsVerified { get; set; }
    public string JwtToken { get; set; } = default!;

    [JsonIgnore]
    public string RefreshToken { get; set; } = default!;
}
