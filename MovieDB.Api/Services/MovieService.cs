using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MovieDB.Api.Entities;
using MovieDB.Api.Helpers;
using MovieDB.Api.Models.Movies;

namespace MovieDB.Api.Services
{
    public interface IMovieService
    {
        public Task<Movie> GetByIdAsync(int id);
        public Task<Movie> CreateAsync(CreateRequest model, Account account);
        public Task<Movie> UpdateAsync(int id, UpdateRequest model);
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

        public async Task<Movie> GetByIdAsync(int id)
        {
            var movie = await _db.Movies.FindAsync(id);
            if (movie is null)
            {
                throw new KeyNotFoundException("Movie does not exist");
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

        public async Task<Movie> UpdateAsync(int id, UpdateRequest model)
        {
            var movie = await _db.Movies.FindAsync(id);
            if (movie is null)
            {
                throw new KeyNotFoundException("Movie does not exist");
            }

            _mapper.Map(model, movie);
            movie.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return movie;
        }
    }
}