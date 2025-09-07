using Microsoft.EntityFrameworkCore;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Http.Requests;
using MovieDB.Api.App.Models;

namespace MovieDB.Api.App.Services;

public interface IConcertService
{
    public IQueryable<Concert> GetAllAsync(Account account);
    public Task<Concert> GetByIdAsync(int id, Account account);
    public Task<Concert> CreateAsync(ConcertCreateRequest model, Account account);
    public Task<Concert> UpdateAsync(int id, ConcertUpdateRequest model, Account account);
    public Task DeleteByIdAsync(int id, Account account);
}

public class ConcertService : IConcertService
{
    private readonly AppDbContext _db;

    public ConcertService(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Concert> GetAllAsync(Account account)
    {
        return _db.Concerts
            .Where(c => c.Account == account && c.DeletedAt == null)
            .OrderByDescending(c => c.SeenAt);
    }

    public async Task<Concert> GetByIdAsync(int id, Account account)
    {
        var concert = await _db.Concerts.FirstOrDefaultAsync(m =>
            m.Id == id &&
            m.Account == account &&
            m.DeletedAt == null);

        if (concert is null)
        {
            throw new KeyNotFoundException($"Cannot find concert with id '{id}' for this account (maybe deleted)");
        }

        return concert;
    }

    public async Task<Concert> CreateAsync(ConcertCreateRequest model, Account account)
    {
        var concert = new Concert
        {
            Title = model.Title!,
            SeenAt = model.SeenAt ?? DateTime.UtcNow,
            Location = model.Location!,
            Genre = (ConcertGenre)model.Genre,
            Rating = (Rating)model.Rating,
            CreatedAt = DateTime.UtcNow,
            Account = account
        };

        _db.Concerts.Add(concert);
        await _db.SaveChangesAsync();

        return concert;
    }

    public async Task<Concert> UpdateAsync(int id, ConcertUpdateRequest model, Account account)
    {
        var concert = await GetByIdAsync(id, account);

        concert.Title = model.Title ?? concert.Title;
        concert.SeenAt = model.SeenAt ?? concert.SeenAt;
        concert.Location = model.Location ?? concert.Location;
        concert.Genre = (ConcertGenre)(model.Genre ?? (int)concert.Genre);
        concert.Rating = (Rating)(model.Rating ?? (int)concert.Rating);
        concert.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return concert;
    }

    public async Task DeleteByIdAsync(int id, Account account)
    {
        var concert = await GetByIdAsync(id, account);

        concert.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }
}
