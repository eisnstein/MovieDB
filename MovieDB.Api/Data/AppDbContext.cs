using Microsoft.EntityFrameworkCore;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account>? Accounts { get; set; }
        public DbSet<Movie>? Movies { get; set; }
        public DbSet<Theater>? Theaters { get; set; }
        public DbSet<Concert>? Concerts { get; set; }
    }

}
