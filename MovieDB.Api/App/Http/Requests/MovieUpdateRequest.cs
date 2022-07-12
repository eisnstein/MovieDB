using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public class MovieUpdateRequest
{
    public string? Title { get; set; }

    [DataType(DataType.Date)]
    public DateTime? SeenAt { get; set; }

    public string? ImdbIdentifier { get; set; }

    public int? Genre { get; set; }

    public int? Rating { get; set; }

    public string? PosterUrl { get; set; }
}
