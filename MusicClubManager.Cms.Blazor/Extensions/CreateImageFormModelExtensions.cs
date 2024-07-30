using MusicClubManager.Cms.Blazor.Interfaces;

namespace MusicClubManager.Cms.Blazor.Extensions
{
    public static class CreateImageFormModelExtensions
    {
        public static MultipartFormDataContent? ToMultipartFormDataContent(this IImageFormModel model)
        {
            if (model.BrowserFile is not { Size: > 0 } file)
            {
                return null;
            }

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.Name);

            content.Add(new StringContent(model.Alt ?? file.Name), "Alt");
            //content.Add(new StringContent(file.ContentType), "ContentType");

            return content;
        }
    }
}
