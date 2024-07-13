using MusicClubManager.Abstractions;
using MusicClubManager.Blazor.Extensions;
using MusicClubManager.Blazor.Models;
using MusicClubManager.Blazor.Services;

namespace MusicClubManager.Blazor.Stores
{
    public class TokenStore(LocalStorageService localStorageService) : ITokenStore
    {
        public async Task<string> GetAccessToken()
        {
            var localStorageToken = await localStorageService.GetItem<LocalStorageToken>("Token");
            
            if(localStorageToken.IsAccessTokenValid() && localStorageToken?.AccessToken is { } accessToken)
            {
                return accessToken;
            }

            return ""; // temp hack, use refresh token

        }

        public async Task RemoveToken()
        {
            await localStorageService.RemoveItem("Token");
        }
    }
}
