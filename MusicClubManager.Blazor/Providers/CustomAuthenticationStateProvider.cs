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

            if (tokens is null || !tokens.IsAccessTokenValid() || string.IsNullOrWhiteSpace(tokens.AccessToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokens.AccessToken);

            var claimsPrincipcal = new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims, "Jwt"));

            var state = new AuthenticationState(claimsPrincipcal);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in jsonElement.EnumerateArray())
                    {
                        claims.Add(new Claim(kvp.Key, element.ToString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                }
            }

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
