using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using System.Net.Http.Json;
using MusicClubManager.Abstractions;

namespace MusicClubManager.Sdk
{
    public class IdentityApiService(IHttpClientFactory httpClientFactory, ITokenStore tokenStore) : IIdentityService
    {
        public async Task<ServiceResult<TokenResult>> GetToken(TokenRequest tokenRequest)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.PostAsJsonAsync("Identity/Token", tokenRequest);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<TokenResult>>() is not { } result)
            {
                return new ServiceResult<TokenResult> { Messages = [new ServiceMessage { Message = "Failed to retrieve the tokens." }] };
            }

            return result;
        }

        public async Task<ServiceResult<string>> Register(RegisterRequest registerRequest)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var accessToken = await tokenStore.GetAccessToken();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var httpResponseMessage = await httpClient.PostAsJsonAsync("Identity/Register", registerRequest);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<string>>() is not { } result)
            {
                return new ServiceResult<string> { Messages = [new ServiceMessage { Message = "Failed to register the user." }] };
            }

            return result;
        }
    }
}
