using System.ComponentModel.DataAnnotations;
using MusicClubManager.Cms.Blazor.Interfaces;

namespace MusicClubManager.Cms.Blazor.Models.FormModels
{
    public class LineupFormModel : ISelectImageFormModel
    {
        public string? Name { get; set; }

        [Required(ErrorMessage = "Doors is a required property.")]
        public DateTime? Doors { get; set; }

        public bool IsSoldOut { get; set; } = false;

        public int? ImageId { get; set; }
    }
}
