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
    public class PerformanceApiService(IHttpClientFactory httpClientFactory) : IPerformanceService
    {
        public async Task<ServiceResult<PerformanceResult>> Create(PerformanceRequest request)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.PostAsJsonAsync("Performance", request);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<PerformanceResult>>() is not { } result)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to create the performance." }],
                };
            }

            return result;
        }

        public async Task<ServiceResult<PerformanceResult>> Delete(int id)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.DeleteAsync("Performance/" + id);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<PerformanceResult>>() is not { } result)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to delete the performance." }],
                };
            }

            return result;
        }

        public async Task<ServiceResult<PerformanceResult>> Get(int id)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Performance/" + id);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<PerformanceResult>>() is not { } result)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to fetch the performance." }],
                };
            }

            return result;
        }

        public async Task<PagedServiceResult<IList<PerformanceResult>>> GetAll(PaginationRequest paginationRequest, PerformanceFilter performanceFilter)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Performance?" + paginationRequest.ToQueryString() + '&' +  performanceFilter.ToQueryString());

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<PagedServiceResult<IList<PerformanceResult>>>() is not { } result)
            {
                return new PagedServiceResult<IList<PerformanceResult>>
                {
                    Messages = [new ServiceMessage { Message = "Failed to fetch the performances." }],
                    Page = paginationRequest.Page,
                    PageSize = paginationRequest.PageSize,
                    TotalCount = 0
                };
            }

            return result;
        }

        public async Task<PagedServiceResult<IList<LineupPerformanceResult>>> GetAll(int id, PaginationRequest paginationRequest, PerformanceFilter filter)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync($"Performance/by-lineup-id/{id}?" + paginationRequest.ToQueryString() + '&' + filter.ToQueryString());

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<PagedServiceResult<IList<LineupPerformanceResult>>>() is not { } result)
            {
                return new PagedServiceResult<IList<LineupPerformanceResult>>
                {
                    Messages = [new ServiceMessage { Message = $"Failed to fetch the performances for lineup {id}." }],
                    Page = paginationRequest.Page,
                    PageSize = paginationRequest.PageSize,
                    TotalCount = 0
                };
            }

            return result;
        }

        public async Task<ServiceResult<PerformanceResult>> Update(int id, PerformanceRequest request)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.PutAsJsonAsync("Performance/" + id, request);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<PerformanceResult>>() is not { } result)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to update the performance." }],
                };
            }

            return result;
        }
    }
}