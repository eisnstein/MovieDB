namespace MovieDB.Api.App.Http.Responses;

public class TheaterResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required DateTime SeenAt { get; set; }
    public required string Location { get; set; }
    public required int Genre { get; set; }
    public required int Rating { get; set; }
}
