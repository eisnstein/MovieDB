using System;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Models.Concerts
{
    public class ConcertResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public string Location { get; set; }
        public ConcertGenre Genre { get; set; }
        public Rating Rating { get; set; }
    }
}
