namespace MovieDB.Api.App.Http.Responses;

public class TheaterResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime SeenAt { get; set; }
    public string Location { get; set; } = default!;
    public int Genre { get; set; }
    public int Rating { get; set; }
}
