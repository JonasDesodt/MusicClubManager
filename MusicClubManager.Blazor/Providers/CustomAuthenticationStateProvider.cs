using Microsoft.AspNetCore.Components.Authorization;
using MusicClubManager.Blazor.Services;
using System.Security.Claims;
using MusicClubManager.Dto.Result;
using MusicClubManager.Blazor.Extensions;
using System.IdentityModel.Tokens.Jwt;
using MusicClubManager.Blazor.Models;
using System.Text.Json;

namespace MusicClubManager.Blazor.Providers
{
    public class CustomAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var tokens = await localStorageService.GetItem<LocalStorageToken>("Token");

            if (tokens is null || !tokens.IsAccessTokenValid())
            {
                await localStorageService.RemoveItem("Token"); // temp hack, don't remove if token is null

                var notAuthenticatedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

                NotifyAuthenticationStateChanged(Task.FromResult(notAuthenticatedState));

                return notAuthenticatedState;      
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokens.AccessToken);

            var claimsPrincipcal = new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims, "Jwt"));

            var authenticatedState = new AuthenticationState(claimsPrincipcal);

            NotifyAuthenticationStateChanged(Task.FromResult(authenticatedState));

            return authenticatedState;
        }
    }
}