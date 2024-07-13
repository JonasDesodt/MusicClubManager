namespace MusicClubManager.Dto.Request
{
    public class ArtistRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}