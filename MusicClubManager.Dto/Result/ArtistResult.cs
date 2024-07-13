namespace MusicClubManager.Dto.Result
{
    public class ArtistResult
    { 
        public required int Id { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}