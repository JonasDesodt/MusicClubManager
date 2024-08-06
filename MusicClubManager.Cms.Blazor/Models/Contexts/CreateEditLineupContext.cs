using MusicClubManager.Cms.Blazor.Interfaces;

namespace MusicClubManager.Cms.Blazor.Models.Contexts
{
    public class CreateEditLineupContext : ISelectImageContext
    {
        public ISelectImageFormModel? Model { get; set; }
    }
}
