using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDB.Shared.Models.Movies
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
        public int? Genre { get; set; }

        [Required]
        public int? Rating { get; set; }
    }
}
