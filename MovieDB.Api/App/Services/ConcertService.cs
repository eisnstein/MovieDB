using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Entities;
using MovieDB.Shared.Models.Concerts;

namespace MovieDB.Api.App.Services;

public interface IConcertService
{
    public IQueryable<Concert> GetAllAsync(Account account);
    public Task<Concert> GetByIdAsync(int id, Account account);
    public Task<Concert> CreateAsync(CreateRequest model, Account account);
    public Task<Concert> UpdateAsync(int id, UpdateRequest model, Account account);
    public Task DeleteByIdAsync(int id, Account account);
}

public class ConcertService : IConcertService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ConcertService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public IQueryable<Concert> GetAllAsync(Account account)
    {
        return _db.Concerts.Where(c => c.Account == account && c.DeletedAt == null).OrderByDescending(c => c.SeenAt);
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

    public async Task<Concert> CreateAsync(CreateRequest model, Account account)
    {
        var concert = _mapper.Map<Concert>(model);
        concert.CreatedAt = DateTime.UtcNow;
        concert.Account = account;

        _db.Concerts.Add(concert);
        await _db.SaveChangesAsync();

        return concert;
    }

    public async Task<Concert> UpdateAsync(int id, UpdateRequest model, Account account)
    {
        var concert = await GetByIdAsync(id, account);

        _mapper.Map(model, concert);
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
