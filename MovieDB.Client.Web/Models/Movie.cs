using System;

namespace MovieDB.Client.Web.Models
{
    public class TMovie
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

    /*public class TMovieGenre
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
    }

    public class TMovie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Imdb { get; set; }
        public TMovieGenre Genre { get; set; }
        public string Rating { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }*/
}
