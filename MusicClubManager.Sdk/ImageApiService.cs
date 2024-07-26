using System.Net.Http.Json;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Sdk.Extensions;

namespace MusicClubManager.Sdk
{
    public class ImageApiService(IHttpClientFactory httpClientFactory)
    {
        public async Task<ServiceResult<ImageResult>?> Create(MultipartFormDataContent content)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var response = await httpClient.PostAsync("Image/Upload", content);
            if (response is not null && response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResult<ImageResult>>();

                return result;
            }

            return null;
        }

        public async Task<ServiceResult<ImageResult>> Get(int id)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Image/" + id);

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResult<ImageResult>>() is not { } result)
            {
                return new ServiceResult<ImageResult>
                {
                    Messages = [new ServiceMessage { Message = "Failed to fetch the images." }],
                };
            }

            return result;
        }

        public async Task<PagedServiceResult<IList<ImageResult>>> GetAll(PaginationRequest paginationRequest)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var httpResponseMessage = await httpClient.GetAsync("Image?" + paginationRequest.ToQueryString());

            if (!httpResponseMessage.IsSuccessStatusCode || await httpResponseMessage.Content.ReadFromJsonAsync<PagedServiceResult<IList<ImageResult>>>() is not { } result)
            {
                return new PagedServiceResult<IList<ImageResult>>
                {
                    Messages = [new ServiceMessage { Message = "Failed to fetch the images." }],
                    Page = paginationRequest.Page,
                    PageSize = paginationRequest.PageSize,
                    TotalCount = 0
                };
            }

            return result;
        }

        public async Task<ServiceResult<ImageResult>?> UpdateProperties(int id, ImageRequest request)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var response = await httpClient.PutAsJsonAsync("Image/Properties/" + id, request);
            if (response is not null && response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResult<ImageResult>>();

                return result;
            }

            return null;
        }

        public async Task<ServiceResult<ImageResult>?> Update(int id, MultipartFormDataContent content)
        {
            var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

            var response = await httpClient.PutAsync("Image/" + id, content);
            if (response is not null && response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResult<ImageResult>>();

                return result;
            }

            return null;
        }
    }
}
