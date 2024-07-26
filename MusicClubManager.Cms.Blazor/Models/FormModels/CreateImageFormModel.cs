using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using MusicClubManager.Cms.Blazor.Interfaces;


namespace MusicClubManager.Cms.Blazor.Models.FormModels
{
    public class CreateImageFormModel : IImageFormModel
    {
        public string? Alt { get; set; }

        [Required(ErrorMessage="File is a required property")]
        public IBrowserFile? BrowserFile { get; set; }
    }
}
