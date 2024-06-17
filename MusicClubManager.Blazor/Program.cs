using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MusicClubManager.Abstractions;
using MusicClubManager.Blazor;
using MusicClubManager.Blazor.Providers;
using MusicClubManager.Blazor.Services;
using MusicClubManager.Blazor.Stores;
using MusicClubManager.Sdk;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("MusicClubManagerApi", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7188");
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MusicClubManagerApi"));

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<ITokenStore, TokenStore>();
builder.Services.AddScoped<IIdentityService, IdentityApiService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

await builder.Build().RunAsync();