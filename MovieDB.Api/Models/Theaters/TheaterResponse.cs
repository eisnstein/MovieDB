using System;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Models.Theaters
{
    public class TheaterResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public string Location { get; set; }
        public TheaterGenre Genre { get; set; }
        public Rating Rating { get; set; }
    }
}
