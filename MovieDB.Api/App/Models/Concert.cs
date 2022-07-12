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
    public string Title { get; set; } = default!;
    public DateTime SeenAt { get; set; }
    public string Location { get; set; } = default!;
    public ConcertGenre Genre { get; set; }
    public Rating Rating { get; set; }

    [JsonIgnore]
    public Account Account { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
