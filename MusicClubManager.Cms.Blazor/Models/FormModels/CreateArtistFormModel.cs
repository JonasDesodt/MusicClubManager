using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Cms.Blazor.Models.FormModels
{
    public class CreateArtistFormModel
    {
        [Required(ErrorMessage = "Name is a required property.")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }

        public IBrowserFile? BrowserFile { get; set; }
    }
}
