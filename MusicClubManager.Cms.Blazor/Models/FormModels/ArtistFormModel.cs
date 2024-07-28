using Microsoft.AspNetCore.Components.Forms;
using MusicClubManager.Cms.Blazor.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Cms.Blazor.Models.FormModels
{
    public class ArtistFormModel 
    {
        [Required(ErrorMessage = "Name is a required property.")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ImageId { get; set; }

        public IBrowserFile? BrowserFile { get; set; }
    }
}
