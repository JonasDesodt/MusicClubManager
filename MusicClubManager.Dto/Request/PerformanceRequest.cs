namespace MusicClubManager.Dto.Request
{
    public class PerformanceRequest
    {
        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }

        public required int ArtistId { get; set; }

        public required int LineupId { get; set; }
    }
}
