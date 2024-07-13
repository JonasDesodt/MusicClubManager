﻿namespace MusicClubManager.Models
{
    public class Event
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        public IList<Lineup> Lineups { get; set; } = [];
    }
}
