using System;
using System.ComponentModel.DataAnnotations;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Models.Movies
{
    public class UpdateRequest
    {
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SeenAt { get; set; }

        public string? ImdbIdentifier { get; set; }

        [EnumDataType(typeof(MovieGenre))]
        public int? Genre { get; set; }

        [EnumDataType(typeof(MovieRating))]
        public int? Rating { get; set; }
    }
}