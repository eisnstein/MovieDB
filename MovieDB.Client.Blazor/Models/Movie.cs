using System;

namespace MovieDB.Client.Blazor.Models
{
    public class Movie
    {
        public int id { get; set; }
        public string genre { get; set; }
        public string imdb { get; set; }
        public string poster { get; set; }
        public int rating { get; set; }
        public DateTime seen_at { get; set; }
        public string title { get; set; }
        public int user_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
}
