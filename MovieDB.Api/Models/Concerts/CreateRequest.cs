using System;
using System.ComponentModel.DataAnnotations;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Models.Concerts
{
    public class CreateRequest
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? SeenAt { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        [EnumDataType(typeof(ConcertGenre))]
        public int? Genre { get; set; }

        [Required]
        [EnumDataType(typeof(Rating))]
        public int? Rating { get; set; }
    }
}
