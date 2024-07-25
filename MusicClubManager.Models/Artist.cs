namespace MusicClubManager.Models
{
    public class Artist
    { 
        public int Id { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }


        public required DateTime Created { get; set; }

        public required DateTime Updated { get; set; }

        public IList<Performance> Performances { get; set; } = [];

        public int? ImageId { get; set; }
        public Image? Image { get; set; }
    }
}