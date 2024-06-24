using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Blazor.Models.FormModels
{
    public class CreatePerformanceFormModel
    {
        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }

        [Required(ErrorMessage = "Selecting an artist is required.")]
        public int? ArtistId { get; set; }
    }
}
