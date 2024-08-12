using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using System.Net.Http.Json;
using System.Net.Http;
using MusicClubManager.Sdk.Extensions;

namespace MusicClubManager.Sdk
{
    public class LineupApiService(IHttpClientFactory httpClientFactory) : ILineupService
    {
        public async Task<ServiceResult<LineupResult>> Create(LineupRequest request)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.PostAsJsonAsync("Lineup", request);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<LineupResult>>() is not { } result)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to create the lineup." }],
                };
            }

            return result;
        }

        public async Task<ServiceResult<LineupResult>> Delete(int id)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.DeleteAsync("Lineup/" + id);

            if(!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<LineupResult>>() is not { } result)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages = [new ServiceMessage { Message = $"Failed to delete lineup with id {id}" }]
                };
            }

            return result;
        }

        public async Task<ServiceResult<LineupResult>> Get(int id)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Lineup/" + id);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<LineupResult>>() is not { } result)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to fetch the lineup." }],
                };
            }

            return result;
        }


        public async Task<ServiceResult<LineupResult>> Get(int id, PaginationRequest paginationRequest)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Lineup/" + id + $"?{paginationRequest.ToQueryString()}");

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<LineupResult>>() is not { } result)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to fetch the lineup." }],
                };
            }

            return result;
        }

        public async Task<ServiceResult<LineupResult>> Previous(int id, PaginationRequest paginationRequest)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Lineup/Previous/" + id + $"?{paginationRequest.ToQueryString()}");

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<LineupResult>>() is not { } result)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to fetch the lineup." }],
                };
            }

            return result;
        }

        public async Task<ServiceResult<LineupResult>> Next(int id, PaginationRequest paginationRequest)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Lineup/Next/" + id + $"?{paginationRequest.ToQueryString()}");

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<LineupResult>>() is not { } result)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to fetch the lineup." }],
                };
            }

            return result;
        }


        public async Task<PagedServiceResult<IList<LineupResult>>> GetAll(PaginationRequest paginationRequest, LineupFilter lineupFilter)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Lineup?" + paginationRequest.ToQueryString() + '&' + lineupFilter.ToQueryString());

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<PagedServiceResult<IList<LineupResult>>>() is not { } result)
            {
                return new PagedServiceResult<IList<LineupResult>>
                {
                    Messages = [new ServiceMessage { Message = "Failed to fetch the lineups." }],
                    Page = paginationRequest.Page,
                    PageSize = paginationRequest.PageSize,
                    TotalCount = 0
                };
            }

            return result;
        }

        public async Task<ServiceResult<LineupResult>> Update(int id, LineupRequest request)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.PutAsJsonAsync("Lineup/" + id, request);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<LineupResult>>() is not { } result)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to update the lineup." }],
                };
            }

            return result;
        }
    }
}