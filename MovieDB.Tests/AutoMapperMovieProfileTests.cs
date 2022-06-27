using System;
using AutoMapper;
using MovieDB.Api.App.Entities;
using MovieDB.Api.App.Helpers;
using MovieDB.Shared.Models.Movies;
using MovieDB.Tests.Factories;
using Xunit;
using UpdateRequest = MovieDB.Shared.Models.Movies.UpdateRequest;

namespace MovieDB.Tests;

public class AutoMapperMovieProfileTests
{
    private readonly IMapper _mapper;

    public AutoMapperMovieProfileTests()
    {
        if (_mapper is null)
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MovieProfile());
            });

            mappingConfig.AssertConfigurationIsValid();

            _mapper = mappingConfig.CreateMapper();
        }
    }

    [Fact]
    public void AutoMapper_Movie_To_MovieResponse()
    {
        var account = AccountFactory.CreateAccount();
        var movie = MovieFactory.CreateMovie(account);

        var response = _mapper.Map<MovieResponse>(movie);

        Assert.Equal("Star Wars", response.Title);
        Assert.Equal(movie.SeenAt, response.SeenAt);
        Assert.Equal(movie.ImdbIdentifier, response.ImdbIdentifier);
        Assert.Equal((int)movie.Rating, response.Rating);
    }

    [Fact]
    public void AutoMapper_UpdateRequest_To_Movie()
    {
        var account = AccountFactory.CreateAccount();
        var movie = MovieFactory.CreateMovie(account);

        var model = new UpdateRequest
        {
            Title = "Star Wars 2",
        };

        var updatedMovie1 = _mapper.Map(model, movie);

        Assert.Equal("Star Wars 2", updatedMovie1.Title);
        Assert.Equal(movie.SeenAt, updatedMovie1.SeenAt);
        Assert.Equal(movie.ImdbIdentifier, updatedMovie1.ImdbIdentifier);
        Assert.Equal(movie.Genre, updatedMovie1.Genre);
        Assert.Equal(movie.Rating, updatedMovie1.Rating);

        // ---

        var newDate = DateTime.UtcNow.AddDays(-1);
        var model2 = new UpdateRequest
        {
            Title = "Star Wars 3",
            SeenAt = newDate
        };

        var updatedMovie2 = _mapper.Map(model2, movie);

        Assert.Equal("Star Wars 3", updatedMovie2.Title);
        Assert.Equal(newDate, updatedMovie2.SeenAt);

        // ---

        var model3 = new UpdateRequest
        {
            Title = "Star Wars 4",
            Genre = 0,
            Rating = 4
        };

        var updatedMovie3 = _mapper.Map(model3, movie);

        Assert.Equal("Star Wars 4", updatedMovie3.Title);
        Assert.Equal(MovieGenre.Action, updatedMovie3.Genre);
        Assert.Equal(Rating.Good, updatedMovie3.Rating);

        // ---

        var model4 = new UpdateRequest
        {
            Title = "",
            Genre = 0,
            Rating = 5
        };

        var updatedMovie4 = _mapper.Map(model4, movie);

        Assert.Equal(movie.Title, updatedMovie4.Title);
        Assert.Equal(MovieGenre.Action, updatedMovie4.Genre);
        Assert.Equal(movie.Rating, updatedMovie4.Rating);
    }
}
