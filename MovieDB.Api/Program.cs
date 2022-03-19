using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MovieDB.Api.Helpers;
using MovieDB.Api.Middleware;
using MovieDB.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(options
    => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ITheaterService, TheaterService>();
builder.Services.AddScoped<IConcertService, ConcertService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Migrate database changes on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<AppDbContext>();
    context!.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieDB.Api v1"));
}

app.UseRouting();

app.UseCors(x => x
    .SetIsOriginAllowed(origin => true)
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
