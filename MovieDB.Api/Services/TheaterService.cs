using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDB.Api.Helpers;
using MovieDB.Api.Entities;
using MovieDB.Shared.Models.Theaters;

namespace MovieDB.Api.Services
{
    public interface ITheaterService
    {
        public IQueryable<Theater> GetAllAsync(Account account);
        public Task<Theater> GetByIdAsync(int id, Account account);
        public Task<Theater> CreateAsync(CreateRequest model, Account account);
        public Task<Theater> UpdateAsync(int id, UpdateRequest model, Account account);
        public Task DeleteByIdAsync(int id, Account account);
    }

    public class TheaterService : ITheaterService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public TheaterService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IQueryable<Theater> GetAllAsync(Account account)
        {
            return _db.Theaters.Where(m => m.Account == account && m.DeletedAt == null);
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

        public async Task<Theater> CreateAsync(CreateRequest model, Account account)
        {
            var theater = _mapper.Map<Theater>(model);
            theater.CreatedAt = DateTime.UtcNow;
            theater.Account = account;

            _db.Theaters.Add(theater);
            await _db.SaveChangesAsync();

            return theater;
        }

        public async Task<Theater> UpdateAsync(int id, UpdateRequest model, Account account)
        {
            var theater = await GetByIdAsync(id, account);

            _mapper.Map(model, theater);
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
}
