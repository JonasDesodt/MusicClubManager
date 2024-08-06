using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Cms.Blazor.Models.FormModels
{
    public class LineupFormModel
    {
        public string? Name { get; set; }

        [Required(ErrorMessage = "Doors is a required property.")]
        public DateTime? Doors { get; set; }

        public bool IsSoldOut { get; set; } = false;

        public int? ImageId { get; set; }
    }
}
