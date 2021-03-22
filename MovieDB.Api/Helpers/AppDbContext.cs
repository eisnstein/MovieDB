using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieDB.Api.Entities;

namespace MovieDB.Api.Helpers
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<Account> Accounts { get; set; }
    }

}