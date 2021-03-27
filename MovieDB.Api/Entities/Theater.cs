using System;
using System.Text.Json.Serialization;

namespace MovieDB.Api.Entities
{
    public enum TheaterGenre
    {
        Action,
        Comedy,
        Drama,
        Fantasy,
        Horror,
        SciFi,
        Thriller
    }

    public enum TheaterRating
    {
        VeryGood,
        Good,
        Ok,
        NotGood,
        Bad
    }

    public class Theater
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public string Location { get; set; }
        public TheaterGenre Genre { get; set; }
        public TheaterRating Rating { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
