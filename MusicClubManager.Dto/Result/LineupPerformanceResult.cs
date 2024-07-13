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

        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }

        public required ArtistResult ArtistResult { get; set; }
    }
}
