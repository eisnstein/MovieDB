using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDB.Shared.Models.Concerts
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
        public int? Genre { get; set; }

        [Required]
        public int? Rating { get; set; }
    }
}
