using Microsoft.EntityFrameworkCore;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Api.App.Models;

namespace MovieDB.Api.App.Services;

public interface ITheaterService
{
    public IQueryable<Theater> GetAllAsync(Account account);
    public Task<Theater> GetByIdAsync(int id, Account account);
    public Task<Theater> CreateAsync(TheaterCreateRequest model, Account account);
    public Task<Theater> UpdateAsync(int id, TheaterUpdateRequest model, Account account);
    public Task DeleteByIdAsync(int id, Account account);
}

public class TheaterService : ITheaterService
{
    private readonly AppDbContext _db;

    public TheaterService(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Theater> GetAllAsync(Account account)
    {
        return _db.Theaters
            .Where(t => t.Account == account && t.DeletedAt == null)
            .OrderByDescending(t => t.SeenAt);
    }

    public async Task<Theater> GetByIdAsync(int id, Account account)
    {
        var theater = await _db.Theaters.FirstOrDefaultAsync(m =>
            m.Id == id &&
            m.Account == account &&
            m.DeletedAt == null);

        if (theater is null)
        {
            throw new KeyNotFoundException($"Cannot find theater entry with id '{id}' for this account (maybe deleted)");
        }

        return theater;
    }

    public async Task<Theater> CreateAsync(TheaterCreateRequest model, Account account)
    {
        var theater = new Theater
        {
            Title = model.Title!,
            SeenAt = model.SeenAt ?? DateTime.UtcNow,
            Location = model.Location!,
            Genre = (TheaterGenre)model.Genre,
            Rating = (Rating)model.Rating,
            CreatedAt = DateTime.UtcNow,
            Account = account
        };

        _db.Theaters.Add(theater);
        await _db.SaveChangesAsync();

        return theater;
    }

    public async Task<Theater> UpdateAsync(int id, TheaterUpdateRequest model, Account account)
    {
        var theater = await GetByIdAsync(id, account);

        theater.Title = model.Title ?? theater.Title;
        theater.SeenAt = model.SeenAt ?? theater.SeenAt;
        theater.Location = model.Location ?? theater.Location;
        theater.Genre = model.Genre is not null ? (TheaterGenre)model.Genre : theater.Genre;
        theater.Rating = model.Rating is not null ? (Rating)model.Rating : theater.Rating;
        theater.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return theater;
    }

    public async Task DeleteByIdAsync(int id, Account account)
    {
        var theater = await GetByIdAsync(id, account);

        theater.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }
}
