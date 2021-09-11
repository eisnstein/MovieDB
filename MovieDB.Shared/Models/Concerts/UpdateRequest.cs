using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDB.Shared.Models.Concerts
{
    public class UpdateRequest
    {
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SeenAt { get; set; }

        public string? Location { get; set; }

        public int? Genre { get; set; }

        public int? Rating { get; set; }
    }
}
