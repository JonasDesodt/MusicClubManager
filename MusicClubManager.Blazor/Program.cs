using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MusicClubManager.Abstractions;
using MusicClubManager.Blazor;
using MusicClubManager.Blazor.Handlers;
using MusicClubManager.Blazor.Providers;
using MusicClubManager.Blazor.Requirements;
using MusicClubManager.Blazor.Services;
using MusicClubManager.Blazor.Stores;
using MusicClubManager.Sdk;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AuthorizationHttpHandler>();
builder.Services.AddHttpClient("MusicClubManagerApi", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7188");
}).AddHttpMessageHandler<AuthorizationHttpHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MusicClubManagerApi"));

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<ITokenStore, TokenStore>();
builder.Services.AddScoped<ArtistFilterStore>();
builder.Services.AddScoped<LineupFilterStore>();
builder.Services.AddScoped<PerformanceFilterStore>();
builder.Services.AddScoped<IIdentityService, IdentityApiService>();
builder.Services.AddScoped<IArtistService, ArtistApiService>();
builder.Services.AddScoped<ILineupService, LineupApiService>();
builder.Services.AddScoped<IPerformanceService, PerformanceApiService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("ValidTokenOnly", policy => policy.Requirements.Add(new ValidTokenRequirement()));

});
builder.Services.AddScoped<IAuthorizationHandler, ValidTokenHandler>();

builder.Services.AddCascadingAuthenticationState();

await builder.Build().RunAsync();