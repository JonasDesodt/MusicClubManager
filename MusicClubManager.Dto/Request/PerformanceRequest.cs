﻿namespace MusicClubManager.Dto.Request
{
    public class PerformanceRequest
    {
        public string? Name { get; set; }
        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }

        public string? Bandcamp { get; set; }
        public string? YouTube { get; set; }
        public string? Spotify { get; set; }

        public required int ArtistId { get; set; }

        public required int LineupId { get; set; }
    }
}
