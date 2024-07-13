namespace MusicClubManager.Models
{
    public class Lineup
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public required DateTime Doors { get; set; }
        public bool IsSoldOut { get; set; }

        public int? EventId { get; set; }
        public Event? Event { get; set; }

        public IList<Performance> Performances { get; set; } = [];
    }
}