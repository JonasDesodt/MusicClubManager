using MusicClubManager.Cms.Blazor.Models.FormModels;
using MusicClubManager.Dto.Result;

namespace MusicClubManager.Cms.Blazor.Extensions
{
    public static class ImageResultExtensions
    {
        public static EditImageFormModel ToEditImageFormModel(this ImageResult result)
        {
            return new EditImageFormModel
            {
                Alt = result.Alt,
                Id = result.Id
            };
        }
    }
}
