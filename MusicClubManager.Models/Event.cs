namespace MusicClubManager.Models
{
    public class Event
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        public required DateTime Created { get; set; }

        public required DateTime Updated { get; set; }


        public IList<Lineup> Lineups { get; set; } = [];
    }
}
