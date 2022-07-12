using Microsoft.EntityFrameworkCore;
using MovieDB.Api.App.Models;

namespace MovieDB.Api.App.Helpers;

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
        var connectionString = _configuration.GetConnectionString("moviedb") ?? throw new ArgumentNullException();
        options.UseSqlite(connectionString);
    }
}
