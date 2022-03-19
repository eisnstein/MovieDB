using System;

namespace MovieDB.Shared.Models.Movies
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public DateTime SeenAt { get; set; }
        public string ImdbIdentifier { get; set; } = default!;
        public int Genre { get; set; }
        public int Rating { get; set; }
        public string? PosterUrl { get; set; }
    }
}
