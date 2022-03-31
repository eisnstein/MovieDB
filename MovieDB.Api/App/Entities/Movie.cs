using System.Text.Json.Serialization;

namespace MovieDB.Api.App.Entities;

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
    public string Title { get; set; } = default!;
    public DateTime SeenAt { get; set; }
    public string ImdbIdentifier { get; set; } = default!;
    public MovieGenre Genre { get; set; }
    public Rating Rating { get; set; }
    public string? PosterUrl { get; set; }
    [JsonIgnore]
    public Account Account { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
