using Microsoft.AspNetCore.Components.Forms;

namespace MusicClubManager.Cms.Blazor.Interfaces
{
    public interface IImageFormModel
    {
        string? Alt { get; set; }

        IBrowserFile? BrowserFile { get; set; }
    }
}
