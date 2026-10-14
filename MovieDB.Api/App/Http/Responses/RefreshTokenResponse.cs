namespace MovieDB.Api.App.Http.Responses;

// The refresh token itself only travels in the HttpOnly cookie
public class RefreshTokenResponse
{
    public required string JwtToken { get; set; }
}
