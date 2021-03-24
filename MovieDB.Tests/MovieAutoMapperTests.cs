using System;
using AutoMapper;
using MovieDB.Api.Entities;
using MovieDB.Api.Helpers;
using MovieDB.Api.Models.Movies;
using Org.BouncyCastle.Crypto.Utilities;
using Xunit;

namespace MovieDB.Tests
{
    public class MovieAutoMapperTests
    {
        private readonly IMapper _mapper;

        public MovieAutoMapperTests()
        {
            if (_mapper is null)
            {
                var mappingConfig = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new AccountProfile());
                    cfg.AddProfile(new MovieProfile());
                });

                _mapper = mappingConfig.CreateMapper();
            }
        }

        [Fact]
        public void MovieToMovieResponseMapping()
        {
            var account = new Account
            {
                Id = 1,
                Title = null,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.test",
                PasswordHash = "asldfjaölskdjfalksjdf",
                AcceptTerms = true,
                Role = Role.Admin,
                VerificationToken = null,
                VerifiedAt = null,
                ResetToken = null,
                ResetTokenExpiresAt = null,
                PasswordResetAt = null,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                UpdatedAt = null,
                RefreshTokens = null
            };

            var movie = new Movie
            {
                Id = 1,
                Title = "Star Wars",
                SeenAt = DateTime.UtcNow.AddDays(-3),
                ImdbIdentifier = "tt123123",
                Genre = MovieGenre.SciFi,
                Rating = MovieRating.Good,
                Account = account,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            var response = _mapper.Map<MovieResponse>(movie);

            Assert.Equal("Star Wars", response.Title);
            Assert.NotNull(response.SeenAt);
            Assert.Equal(MovieGenre.SciFi, response.Genre);
        }

        [Fact]
        public void UpdateRequestToMovie()
        {
            var account = new Account
            {
                Id = 1,
                Title = null,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.test",
                PasswordHash = "asldfjaölskdjfalksjdf",
                AcceptTerms = true,
                Role = Role.Admin,
                VerificationToken = null,
                VerifiedAt = null,
                ResetToken = null,
                ResetTokenExpiresAt = null,
                PasswordResetAt = null,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                UpdatedAt = null,
                RefreshTokens = null
            };

            var movie = new Movie
            {
                Id = 1,
                Title = "Star Wars",
                SeenAt = DateTime.UtcNow.AddDays(-3),
                ImdbIdentifier = "tt123123",
                Genre = MovieGenre.SciFi,
                Rating = MovieRating.Good,
                Account = account,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            var model = new UpdateRequest
            {
                Title = "Star Wars 2",
            };

            var updatedMovie = _mapper.Map(model, movie);

            Assert.Equal("Star Wars 2", updatedMovie.Title);
        }
    }
}
