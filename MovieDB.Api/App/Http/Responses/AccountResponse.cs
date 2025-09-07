namespace MovieDB.Api.App.Http.Responses;

public class AccountResponse
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsVerified { get; set; }
}
