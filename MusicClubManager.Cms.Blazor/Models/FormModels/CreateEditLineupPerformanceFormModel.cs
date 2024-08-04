using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Cms.Blazor.Models.FormModels
{
    public class CreateEditLineupPerformanceFormModel
    {
        public string? Name { get; set; }
        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }

        public string? Bandcamp { get; set; }
        public string? YouTube { get; set; }
        public string? Spotify { get; set; }


        [Required(ErrorMessage = "Selecting an artist is required.")]
        public int? ArtistId { get; set; }
         
        [Required(ErrorMessage = "Select in a lineup is required.")]
        public int? LineupId { get; set; }
    }
}