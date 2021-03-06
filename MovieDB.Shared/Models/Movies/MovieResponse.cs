using System;

namespace MovieDB.Shared.Models.Movies
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public string ImdbIdentifier { get; set; }
        public int Genre { get; set; }
        public int Rating { get; set; }
    }
}
