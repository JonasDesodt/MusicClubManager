namespace MusicClubManager.Dto.Result
{
    public class PerformanceResult
    {
        public required int Id { get; set; }

        public string? Name { get; set; }
        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }

        public string? Bandcamp { get; set; }
        public string? YouTube { get; set; }
        public string? Spotify { get; set; }

        public required ArtistResult ArtistResult { get; set; }

        public required PerformanceLineupResult LineupResult { get; set; }
    }
}