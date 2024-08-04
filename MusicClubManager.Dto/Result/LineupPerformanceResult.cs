using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Dto.Result
{
    //should have a LineupResult as parent
    public class LineupPerformanceResult
    {
        public required int Id { get; set; }

        public string? Name { get; set; }
        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }

        public string? Bandcamp { get; set; }
        public string? YouTube { get; set; }
        public string? Spotify { get; set; }

        public required ArtistResult ArtistResult { get; set; }
    }
}
