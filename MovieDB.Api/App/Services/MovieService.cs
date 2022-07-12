using AutoMapper;
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
    private readonly IMapper _mapper;

    public MovieService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
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
        var movie = _mapper.Map<Movie>(model);
        var now = DateTime.UtcNow;

        movie.CreatedAt = now;
        movie.UpdatedAt = now;
        movie.Account = account;

        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();

        return movie;
    }

    public async Task<Movie> UpdateAsync(int id, MovieUpdateRequest model, Account account)
    {
        var movie = await GetByIdAsync(id, account);

        _mapper.Map(model, movie);
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
