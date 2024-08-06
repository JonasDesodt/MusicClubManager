using Microsoft.AspNetCore.Components.Forms;

namespace MusicClubManager.Cms.Blazor.Interfaces
{
    public interface IArtistFormModel : ISelectImageFormModel
    {
        string? Name { get; set; }
        string? Description { get; set; }
        //int? ImageId { get; set; }

        IBrowserFile? BrowserFile { get; set; }
    }
}
