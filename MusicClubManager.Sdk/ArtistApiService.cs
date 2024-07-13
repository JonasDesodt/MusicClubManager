using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Sdk.Extensions;
using System.Net.Http.Json;

namespace MusicClubManager.Sdk
{
    public class ArtistApiService(IHttpClientFactory httpClientFactory) : IArtistService
    {
        public async Task<ServiceResult<ArtistResult>> Create(ArtistRequest request)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.PostAsJsonAsync("Artist", request);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<ArtistResult>>() is not { } result)
            {
                return new ServiceResult<ArtistResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to create the artist." }],
                };
            }

            return result;
        }

        public async Task<ServiceResult<ArtistResult>> Delete(int id)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.DeleteAsync("Artist/" + id);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<ArtistResult>>() is not { } result)
            {
                return new ServiceResult<ArtistResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to delete the artist." }],
                };
            }

            return result;
        }

        public async Task<ServiceResult<ArtistResult>> Get(int id)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Artist/" + id);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<ArtistResult>>() is not { } result)
            {
                return new ServiceResult<ArtistResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to fetch the artist." }],
                };
            }

            return result;
        }

        public async Task<PagedServiceResult<IList<ArtistResult>>> GetAll(PaginationRequest paginationRequest, ArtistFilter artistFilter)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Artist?" + paginationRequest.ToQueryString() + '&' + artistFilter.ToQueryString());

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<PagedServiceResult<IList<ArtistResult>>>() is not { } result)
            {
                return new PagedServiceResult<IList<ArtistResult>> 
                { 
                    Messages = [new ServiceMessage { Message = "Failed to fetch the artists." }],
                    Page = paginationRequest.Page,
                    PageSize = paginationRequest.PageSize,
                    TotalCount = 0                    
                };
            }

            return result;
        }

        public async Task<ServiceResult<ArtistResult>> Update(int id, ArtistRequest request)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.PutAsJsonAsync("Artist/" + id, request);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<ArtistResult>>() is not { } result)
            {
                return new ServiceResult<ArtistResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to update the artist." }],
                };
            }

            return result;
        }
    }
}