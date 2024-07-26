using Microsoft.AspNetCore.Components.Forms;
using MusicClubManager.Cms.Blazor.Interfaces;

namespace MusicClubManager.Cms.Blazor.Models.FormModels
{
    public class EditImageFormModel : IImageFormModel
    {
        public string? Alt { get; set; }

        public IBrowserFile? BrowserFile { get; set; }
    }
}
