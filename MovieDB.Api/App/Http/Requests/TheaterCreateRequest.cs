using System.ComponentModel.DataAnnotations;

namespace MovieDB.Api.App.Http.Requests;

public class TheaterCreateRequest
{
    [Required]
    public string? Title { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? SeenAt { get; set; }

    [Required]
    public string? Location { get; set; }

    [Required]
    public int? Genre { get; set; }

    [Required]
    public int? Rating { get; set; }
}
