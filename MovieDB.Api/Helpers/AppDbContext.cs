using Microsoft.EntityFrameworkCore;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Helpers;

public sealed class AppDbContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Theater> Theaters => Set<Theater>();
    public DbSet<Concert> Concerts => Set<Concert>();

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
