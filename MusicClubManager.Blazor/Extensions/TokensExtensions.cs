using MusicClubManager.Blazor.Models;
using MusicClubManager.Dto.Result;

namespace MusicClubManager.Blazor.Extensions
{
    public static class LocalStorageTokenExtensions
    {
        public static bool IsAccessTokenValid(this LocalStorageToken? localStorageToken)
        {
            if(localStorageToken is null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(localStorageToken.AccessToken))
            {
                return false;
            }

            if (localStorageToken.Received.AddSeconds(localStorageToken.ExpiresIn) <= DateTime.UtcNow.AddSeconds(-5))
            {
                return false;
            }

            return true;
        }
    }
}
