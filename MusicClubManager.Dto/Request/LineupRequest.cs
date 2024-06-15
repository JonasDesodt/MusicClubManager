namespace MusicClubManager.Dto.Request
{
    public class LineupRequest
    {
        public string? Name { get; set; }
        public required DateTime Doors { get; set; }
        public bool IsSoldOut { get; set; } = false;

        public int? EventId { get; set; }
    }
}