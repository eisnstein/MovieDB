namespace MovieDB.Api.App.Http.Responses;

public record AuthenticateResponse
{
    public required int Id { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
    public bool IsVerified { get; init; }
    public required string JwtToken { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
