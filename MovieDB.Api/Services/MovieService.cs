using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDB.Api.Entities;
using MovieDB.Api.Helpers;
using MovieDB.Api.Models.Movies;

namespace MovieDB.Api.Services
{
    public interface IMovieService
    {
        public IQueryable<Movie> GetAllAsync(Account account);
        public Task<Movie> GetByIdAsync(int id, Account account);
        public Task<Movie> CreateAsync(CreateRequest model, Account account);
        public Task<Movie> UpdateAsync(int id, UpdateRequest model, Account account);
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
            return _db.Movies.Where(m => m.Account == account);
        }

        public async Task<Movie> GetByIdAsync(int id, Account account)
        {
            var movie = await _db.Movies.FirstOrDefaultAsync(m => m.Id == id && m.Account == account);
            if (movie is null)
            {
                throw new KeyNotFoundException($"Cannot find movie with id '{id}' for this account");
            }

            return movie;
        }

        public async Task<Movie> CreateAsync(CreateRequest model, Account account)
        {
            var movie = _mapper.Map<Movie>(model);
            movie.CreatedAt = DateTime.UtcNow;
            movie.Account = account;

            _db.Movies.Add(movie);
            await _db.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie> UpdateAsync(int id, UpdateRequest model, Account account)
        {
            var movie = await _db.Movies.FirstOrDefaultAsync(m => m.Id == id && m.Account == account);
            if (movie is null)
            {
                throw new KeyNotFoundException($"Cannot find movie with id '{id}' for this account");
            }

            _mapper.Map(model, movie);
            movie.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return movie;
        }
    }
}
