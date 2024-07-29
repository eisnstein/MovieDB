using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MovieDB.Api.App.Helpers;
using MovieDB.Api.App.Middleware;
using MovieDB.Api.App.Services;
using MovieDB.Api.Routes;

namespace MovieDB.Api.Bootstrap;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var appSettings = builder.Configuration.GetSection(nameof(AppSettings));
        var secretKey = appSettings["Secret"] ?? throw new Exception("No secret key for JWT token given");

        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.Configure<AppSettings>(appSettings);
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true
                };
            });
        builder.Services.AddCors();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddScoped<ITheaterService, TheaterService>();
        builder.Services.AddScoped<IConcertService, ConcertService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        builder.Services.AddRouting(options => options.LowercaseUrls = true);
    }

    public static void ConfigureMiddlewares(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseCors(x => x
            .SetIsOriginAllowed(_ => true)
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<AttachUserMiddleware>();
    }

    public static void ConfigureRoutes(WebApplication app)
    {
        ApiRoutes.Definitions(app);
    }
}
