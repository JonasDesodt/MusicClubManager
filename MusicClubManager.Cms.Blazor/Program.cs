using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MusicClubManager.Abstractions;
using MusicClubManager.Cms.Blazor;
using MusicClubManager.Sdk;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("MusicClubManagerApi", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7188");
});

builder.Services.AddScoped<IArtistService, ArtistApiService>();
builder.Services.AddScoped<ILineupService, LineupApiService>();
builder.Services.AddScoped<IPerformanceService, PerformanceApiService>();

await builder.Build().RunAsync();
