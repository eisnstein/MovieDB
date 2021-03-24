using System;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Models.Movies
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public string ImdbIdentifier { get; set; }
        public MovieGenre Genre { get; set; }
        public MovieRating Rating { get; set; }
    }
}