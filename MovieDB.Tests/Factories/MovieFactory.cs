using System;
using MovieDB.Api.Entities;

namespace MovieDB.Tests.Factories
{
    public class MovieFactory
    {
        public static Movie CreateMovie(Account account)
        {
            return new Movie
            {
                Id = 1,
                Title = "Star Wars",
                SeenAt = DateTime.UtcNow.AddDays(-3),
                ImdbIdentifier = "tt123123",
                Genre = MovieGenre.SciFi,
                Rating = Rating.Good,
                Account = account,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };
        }
    }
}
