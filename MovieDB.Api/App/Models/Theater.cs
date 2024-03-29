using System.Text.Json.Serialization;

namespace MovieDB.Api.App.Models;

public enum TheaterGenre
{
    Opera,
    Operette,
    Musical,
    Theater,
    Ballet
}

public class Theater
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime SeenAt { get; set; }
    public string Location { get; set; } = default!;
    public TheaterGenre Genre { get; set; }
    public Rating Rating { get; set; }
    [JsonIgnore]
    public Account Account { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
