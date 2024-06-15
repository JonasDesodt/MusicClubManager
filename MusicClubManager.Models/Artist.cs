namespace MusicClubManager.Models
{
    public class Artist
    { 
        public int Id { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }

        public IList<Performance> Performances { get; set; } = [];
    }
}