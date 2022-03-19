using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDB.Shared.Models.Concerts
{
    public class CreateRequest
    {
        [Required]
        public string Title { get; set; } = default!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime SeenAt { get; set; } = default!;

        [Required]
        public string Location { get; set; } = default!;

        [Required]
        public int Genre { get; set; } = default!;

        [Required]
        public int Rating { get; set; } = default!;
    }
}
