using Microsoft.EntityFrameworkCore;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Api.App.Models;

namespace MovieDB.Api.App.Services;

public interface IMovieService
{
    public IQueryable<Movie> GetAllAsync(Account account);
    public Task<Movie> GetByIdAsync(int id, Account account);
    public Task<Movie> CreateAsync(MovieCreateRequest model, Account account);
    public Task<Movie> UpdateAsync(int id, MovieUpdateRequest model, Account account);
    public Task DeleteByIdAsync(int id, Account account);
}

public class MovieService : IMovieService
{
    private readonly AppDbContext _db;

    public MovieService(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Movie> GetAllAsync(Account account)
    {
        return _db.Movies
            .Where(m => m.Account == account && m.DeletedAt == null)
            .OrderByDescending(m => m.SeenAt);
    }

    public async Task<Movie> GetByIdAsync(int id, Account account)
    {
        var movie = await _db.Movies.FirstOrDefaultAsync(m =>
            m.Id == id &&
            m.Account == account &&
            m.DeletedAt == null);

        if (movie is null)
        {
            throw new KeyNotFoundException($"Cannot find movie with id '{id}' for this account (maybe deleted)");
        }

        return movie;
    }

    public async Task<Movie> CreateAsync(MovieCreateRequest model, Account account)
    {
        var movie = new Movie
        {
            Title = model.Title!,
            SeenAt = model.SeenAt ?? DateTime.UtcNow,
            ImdbIdentifier = model.ImdbIdentifier!,
            Genre = (MovieGenre)model.Genre,
            Rating = (Rating)model.Rating,
            PosterUrl = model.PosterUrl,
            CreatedAt = DateTime.UtcNow,
            Account = account
        };

        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();

        return movie;
    }

    public async Task<Movie> UpdateAsync(int id, MovieUpdateRequest model, Account account)
    {
        var movie = await GetByIdAsync(id, account);

        movie.Title = model.Title ?? movie.Title;
        movie.SeenAt = model.SeenAt ?? movie.SeenAt;
        movie.ImdbIdentifier = model.ImdbIdentifier ?? movie.ImdbIdentifier;
        movie.Genre = (MovieGenre)(model.Genre ?? (int)movie.Genre);
        movie.Rating = (Rating)(model.Rating ?? (int)movie.Rating);
        movie.PosterUrl = model.PosterUrl ?? movie.PosterUrl;
        movie.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return movie;
    }

    public async Task DeleteByIdAsync(int id, Account account)
    {
        var movie = await GetByIdAsync(id, account);

        movie.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }
}
