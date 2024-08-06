using MusicClubManager.Cms.Blazor.Models.FormModels;
using MusicClubManager.Cms.Blazor.Interfaces;

namespace MusicClubManager.Cms.Blazor.Models.Contexts
{
    public class CreateArtistContext : ISelectImageContext
    {
        public required ISelectImageFormModel Model { get; set; }
    }
}
