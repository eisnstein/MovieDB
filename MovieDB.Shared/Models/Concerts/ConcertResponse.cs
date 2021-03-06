using System;

namespace MovieDB.Shared.Models.Concerts
{
    public class ConcertResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public string Location { get; set; }
        public int Genre { get; set; }
        public int Rating { get; set; }
    }
}
