using System.Text.Json.Serialization;

namespace MovieDB.Api.App.Models;

public enum MovieGenre
{
    Action,
    Comedy,
    Drama,
    Fantasy,
    Horror,
    SciFi,
    Thriller
}

public class Movie
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required DateTime SeenAt { get; set; }
    public required string ImdbIdentifier { get; set; }
    public required MovieGenre Genre { get; set; }
    public required Rating Rating { get; set; }
    public string? PosterUrl { get; set; }

    [JsonIgnore]
    public required Account Account { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
