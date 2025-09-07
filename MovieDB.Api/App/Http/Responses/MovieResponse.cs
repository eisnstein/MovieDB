namespace MovieDB.Api.App.Http.Responses;

public class MovieResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required DateTime SeenAt { get; set; }
    public required string ImdbIdentifier { get; set; }
    public required int Genre { get; set; }
    public required int Rating { get; set; }
    public string? PosterUrl { get; set; }
}
