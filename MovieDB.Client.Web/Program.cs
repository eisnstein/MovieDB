using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MovieDB.Client.Web.Services;
using MovieDB.Client.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services
    .AddScoped<IAccountService, AccountService>()
    .AddScoped<IAlertService, AlertService>()
    .AddScoped<ILocalStorageService, LocalStorageService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:4001") });

await builder.Build().RunAsync();
