using MusicClubManager.Cms.Blazor.Interfaces;

namespace MusicClubManager.Cms.Blazor.Models.Contexts
{
    public class CreateEditArtistContext : ISelectImageContext
    {
        public ISelectImageFormModel? Model { get; set; }
    }
}
