namespace MusicClubManager.Models
{
    public class Performance
    {
        public int Id { get; set; }

        //todo: name => this should be the band name? a performance should then have 1 of more artists
        //public required Name {get;set;}
        //public string? Image {get;set;}

        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }

        public required int ArtistId { get; set; }
        public Artist? Artist { get; set; }

        public required DateTime Created { get; set; }

        public required DateTime Updated { get; set; }

        public required int LineupId { get; set; }
        public Lineup? Lineup { get; set; }
    }
}