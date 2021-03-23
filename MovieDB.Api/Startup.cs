using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MovieDB.Api.Helpers;
using MovieDB.Api.Middleware;
using MovieDB.Api.Services;

namespace MovieDB.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
            services.AddSingleton<IAppSettings>(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieDB.Api", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieDB.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}