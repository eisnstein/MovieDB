using AutoMapper;
using MovieDB.Api.App.Entities;
using MovieDB.Api.App.Services;
using MovieDB.Shared.Models.Movies;
using MovieCreateRequest = MovieDB.Shared.Models.Movies.CreateRequest;
using MovieUpdateRequest = MovieDB.Shared.Models.Movies.UpdateRequest;

namespace MovieDB.Api.App.Endpoints;

public static class MoviesEndpoints
{
    public static IResult GetAll(
        HttpContext context,
        IMapper mapper,
        IMovieService movieService)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var movies = movieService.GetAllAsync(account);
        var mapped = mapper.Map<List<MovieResponse>>(movies);

        return Results.Ok(mapped);
    }

    public static async Task<IResult> GetById(
        HttpContext context,
        IMapper mapper,
        IMovieService movieService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var movie = await movieService.GetByIdAsync(id, account);

        return Results.Ok(mapper.Map<MovieResponse>(movie));
    }

    public static async Task<IResult> Create(
        HttpContext context,
        IMapper mapper,
        IMovieService movieService,
        MovieCreateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var movie = await movieService.CreateAsync(model, account);
        var response = mapper.Map<MovieResponse>(movie);

        return Results.CreatedAtRoute(nameof(GetById), new { id = movie.Id }, response);
    }

    public static async Task<IResult> Update(
        HttpContext context,
        IMapper mapper,
        IMovieService movieService,
        int id,
        MovieUpdateRequest model)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        var movie = await movieService.UpdateAsync(id, model, account);
        var response = mapper.Map<MovieResponse>(movie);

        return Results.CreatedAtRoute(nameof(GetById), new { id = movie.Id }, response);
    }

    public static async Task<IResult> Delete(
        HttpContext context,
        IMovieService movieService,
        int id)
    {
        var account = (Account) context.Items[nameof(Account)]!;
        await movieService.DeleteByIdAsync(id, account);

        return Results.NoContent();
    }
}
