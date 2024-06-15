namespace MusicClubManager.Models
{
    public class Performance
    {
        public int Id { get; set; }

        public DateTime? Start { get; set; }
        public uint? Duration { get; set; }
        public string? Type { get; set; }

        public required int ArtistId { get; set; }
        public Artist? Artist { get; set; }

        public required int LineupId { get; set; }
        public Lineup? Lineup { get; set; }
    }
}