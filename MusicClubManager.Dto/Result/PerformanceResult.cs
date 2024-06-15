namespace MusicClubManager.Dto.Result
{
    public class PerformanceResult
    {
        public required int Id { get; set; }

        public DateTime? Start { get; set; }
        public uint? Duration { get; set; }
        public string? Type { get; set; }

        public ArtistResult? Artist { get; set; }

        public LineupResult? Lineup { get; set; }
    }
}