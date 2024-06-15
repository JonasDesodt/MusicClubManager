namespace MusicClubManager.Dto.Request
{
    public class EventRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}