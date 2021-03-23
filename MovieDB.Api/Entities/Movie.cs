using System;

namespace MovieDB.Api.Entities
{
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

    public enum MovieRating
    {
        VeryGood,
        Good,
        Ok,
        NotGood,
        Bad
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public string ImdbIdentifier { get; set; }
        public MovieGenre Genre { get; set; }
        public MovieRating Rating { get; set; }
        public Account Account { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}