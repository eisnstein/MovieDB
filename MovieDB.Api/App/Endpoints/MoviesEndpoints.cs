using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MovieDB.Api.App.Entities;
using MovieDB.Api.App.Services;
using MovieDB.Shared.Models.Movies;
using MovieCreateRequest = MovieDB.Shared.Models.Movies.CreateRequest;
using MovieUpdateRequest = MovieDB.Shared.Models.Movies.UpdateRequest;

namespace MovieDB.Api.App.Endpoints;

public static class MoviesEndpoints
{
    public static Ok<List<MovieResponse>> GetAll(
        HttpContext context,
        IMapper mapper,
        IMovieService movieService)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var movies = movieService.GetAllAsync(account);
        var mapped = mapper.Map<List<MovieResponse>>(movies);

        return TypedResults.Ok(mapped);
    }

    public static async Task<Ok<MovieResponse>> GetById(
        HttpContext context,
        IMapper mapper,
        IMovieService movieService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var movie = await movieService.GetByIdAsync(id, account);
        var response = mapper.Map<MovieResponse>(movie);

        return TypedResults.Ok(response);
    }

    public static async Task<CreatedAtRoute<MovieResponse>> Create(
        HttpContext context,
        IMapper mapper,
        IMovieService movieService,
        MovieCreateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var movie = await movieService.CreateAsync(model, account);
        var response = mapper.Map<MovieResponse>(movie);

        return TypedResults.CreatedAtRoute(response, nameof(GetById));
    }

    public static async Task<CreatedAtRoute<MovieResponse>> Update(
        HttpContext context,
        IMapper mapper,
        IMovieService movieService,
        int id,
        MovieUpdateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var movie = await movieService.UpdateAsync(id, model, account);
        var response = mapper.Map<MovieResponse>(movie);

        return TypedResults.CreatedAtRoute(response, nameof(GetById));
    }

    public static async Task<NoContent> Delete(
        HttpContext context,
        IMovieService movieService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        await movieService.DeleteByIdAsync(id, account);

        return TypedResults.NoContent();
    }
}
