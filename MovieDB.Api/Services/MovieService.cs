using System;
using System.Threading.Tasks;
using AutoMapper;
using MovieDB.Api.Entities;
using MovieDB.Api.Helpers;
using MovieDB.Api.Models.Movies;

namespace MovieDB.Api.Services
{
    public interface IMovieService
    {
        public Task<Movie> UpdateAsync(CreateRequest model, Account account);
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

        public async Task<Movie> UpdateAsync(CreateRequest model, Account account)
        {
            var movie = _mapper.Map<Movie>(model);
            movie.CreatedAt = DateTime.UtcNow;
            movie.Account = account;

            _db.Movies.Add(movie);
            await _db.SaveChangesAsync();

            return movie;
        }
    }
}