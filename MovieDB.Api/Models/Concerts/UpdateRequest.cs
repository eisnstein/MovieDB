using System;
using System.ComponentModel.DataAnnotations;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Models.Concerts
{
    public class UpdateRequest
    {
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SeenAt { get; set; }

        public string? Location { get; set; }

        [EnumDataType(typeof(ConcertGenre))]
        public int? Genre { get; set; }

        [EnumDataType(typeof(Rating))]
        public int? Rating { get; set; }
    }
}
