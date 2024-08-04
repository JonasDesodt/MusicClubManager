namespace MusicClubManager.Models
{
    public class Performance
    {
        public int Id { get; set; }

        //todo: name => this should be the band name? a performance should then have 1 of more artists
        //public required Name {get;set;}
        public string? Name { get; set; }
        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }

        public required int ArtistId { get; set; }
        public Artist? Artist { get; set; }

        public string? Bandcamp { get; set; }

        public string? YouTube { get; set; }

        public string? Spotify { get; set; }

        public required DateTime Created { get; set; }

        public required DateTime Updated { get; set; }

        public required int LineupId { get; set; }
        public Lineup? Lineup { get; set; }
    }
}