using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Cms.Blazor.Models.FormModels
{
    public class CreateEditLineupPerformanceFormModel
    {
        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }

        [Required(ErrorMessage = "Selecting an artist is required.")]
        public int? ArtistId { get; set; }
         
        [Required(ErrorMessage = "Select in a lineup is required.")]
        public int? LineupId { get; set; }
    }
}