using System.Text.Json.Serialization;

namespace MovieDB.Api.App.Models;

public enum ConcertGenre
{
    Rock,
    Classic,
    Reggae,
    Pop,
    Latin,
    Electro,
    DrumnBass
}

public class Concert
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required DateTime SeenAt { get; set; }
    public required string Location { get; set; }
    public required ConcertGenre Genre { get; set; }
    public required Rating Rating { get; set; }

    [JsonIgnore]
    public required Account Account { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
