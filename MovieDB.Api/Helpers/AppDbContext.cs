using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Helpers
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account>? Accounts { get; set; }
        public DbSet<Movie>? Movies { get; set; }
        public DbSet<Theater>? Theaters { get; set; }
        public DbSet<Concert>? Concerts { get; set; }

        private readonly IConfiguration _configuration;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(_configuration.GetConnectionString("moviedb"));
        }
    }

}
