﻿using System.Net.Http.Json;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;

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


        //public async Task<string?> Create(MultipartFormDataContent content)
        //{
        //    var httpClient = httpClientFactory.CreateClient("MusicClubManagerApi");

        //    //var content = new MultipartFormDataContent();
        //    //var fileContent = new StreamContent(image.OpenReadStream());
        //    //fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
        //    //content.Add(fileContent, "file", image.Name);

        //    var response = await httpClient.PostAsync("Image", content);
        //    if (response is not null && response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadFromJsonAsync<UploadResult>();

        //        return result?.Url;
        //    }

        //    return null;
        //}
    }
}
