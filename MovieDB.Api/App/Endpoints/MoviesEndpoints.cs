using Microsoft.AspNetCore.Http.HttpResults;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Api.App.Http.Responses;
using MovieDB.Api.App.Models;
using MovieDB.Api.App.Services;

namespace MovieDB.Api.App.Endpoints;

public static class MoviesEndpoints
{
    public static Ok<List<MovieResponse>> GetAll(
        HttpContext context,
        IMovieService movieService)
    {
        var account = (Account)context.Items[nameof(Account)]!;
        var movies = movieService.GetAllAsync(account);

        var response = movies.Select(m => new MovieResponse
        {
            Id = m.Id,
            Title = m.Title,
            SeenAt = m.SeenAt,
            ImdbIdentifier = m.ImdbIdentifier,
            Genre = (int)m.Genre,
            Rating = (int)m.Rating,
            PosterUrl = m.PosterUrl,
        }).ToList();

        return TypedResults.Ok(response);
    }

    public static async Task<Ok<MovieResponse>> GetById(
        HttpContext context,
        IMovieService movieService,
        int id)
    {
        var account = (Account)context.Items[nameof(Account)]!;
        var movie = await movieService.GetByIdAsync(id, account);

        var response = new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            SeenAt = movie.SeenAt,
            ImdbIdentifier = movie.ImdbIdentifier,
            Genre = (int)movie.Genre,
            Rating = (int)movie.Rating,
            PosterUrl = movie.PosterUrl,
        };

        return TypedResults.Ok(response);
    }

    public static async Task<CreatedAtRoute<MovieResponse>> Create(
        HttpContext context,
        IMovieService movieService,
        MovieCreateRequest model)
    {
        var account = (Account)context.Items[nameof(Account)]!;
        var movie = await movieService.CreateAsync(model, account);

        var response = new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            SeenAt = movie.SeenAt,
            ImdbIdentifier = movie.ImdbIdentifier,
            Genre = (int)movie.Genre,
            Rating = (int)movie.Rating,
            PosterUrl = movie.PosterUrl,
        };

        return TypedResults.CreatedAtRoute(response, nameof(GetById));
    }

    public static async Task<CreatedAtRoute<MovieResponse>> Update(
        HttpContext context,
        IMovieService movieService,
        int id,
        MovieUpdateRequest model)
    {
        var account = (Account)context.Items[nameof(Account)]!;
        var movie = await movieService.UpdateAsync(id, model, account);

        var response = new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            SeenAt = movie.SeenAt,
            ImdbIdentifier = movie.ImdbIdentifier,
            Genre = (int)movie.Genre,
            Rating = (int)movie.Rating,
            PosterUrl = movie.PosterUrl,
        };

        return TypedResults.CreatedAtRoute(response, nameof(GetById));
    }

    public static async Task<NoContent> Delete(
        HttpContext context,
        IMovieService movieService,
        int id)
    {
        var account = (Account)context.Items[nameof(Account)]!;
        await movieService.DeleteByIdAsync(id, account);

        return TypedResults.NoContent();
    }
}
