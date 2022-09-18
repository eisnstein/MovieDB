using Microsoft.EntityFrameworkCore;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.Bootstrap;

var builder = WebApplication.CreateBuilder(args);
Startup.ConfigureServices(builder);

var app = builder.Build();

// Migrate database changes on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (app.Environment.IsEnvironment("Test"))
    {
        // Reset database
        context.Database.EnsureDeleted();
    }

    context.Database.Migrate();
}

Startup.ConfigureMiddlewares(app);
Startup.ConfigureRoutes(app);

app.Run();

public partial class Program {}
