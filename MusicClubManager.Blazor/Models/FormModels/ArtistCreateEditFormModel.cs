using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Blazor.Models.FormModels
{
    public class ArtistCreateEditFormModel
    {
        [Required(ErrorMessage="Name is a required property.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }
    }
}
