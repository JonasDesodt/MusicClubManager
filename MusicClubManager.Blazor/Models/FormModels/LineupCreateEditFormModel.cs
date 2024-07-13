using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Blazor.Models.FormModels
{
    public class LineupCreateEditFormModel
    {
        public string? Name { get; set; }

        [Required(ErrorMessage = "Doors is a required property.")]
        public DateTime? Doors { get; set; }

        public bool IsSoldOut { get; set; } = false;

        public int? EventId { get; set; }
    }
}
