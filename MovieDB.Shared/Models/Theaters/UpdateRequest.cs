using System;
using System.ComponentModel.DataAnnotations;

namespace MovieDB.Shared.Models.Theaters
{
    public class UpdateRequest
    {
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SeenAt { get; set; }

        public string? ImdbIdentifier { get; set; }

        public int? Genre { get; set; }

        public int? Rating { get; set; }
    }
}
