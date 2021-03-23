using System;
using System.ComponentModel.DataAnnotations;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Models.Movies
{
    public class CreateRequest
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? SeenAt { get; set; }

        [Required]
        public string? ImdbIdentifier { get; set; }

        [Required]
        [EnumDataType(typeof(MovieGenre))]
        public int? Genre { get; set; }

        [Required]
        [EnumDataType(typeof(MovieRating))]
        public int? Rating { get; set; }
    }
}