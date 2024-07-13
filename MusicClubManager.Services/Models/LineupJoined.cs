using MusicClubManager.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Services.Models
{
    internal class LineupJoined
    {
        public required int Id { get; set; }

        public string? Name { get; set; }
        public required DateTime Doors { get; set; }
        public bool IsSoldOut { get; set; }

        public EventResult? Event { get; set; }

        public IList<PerformanceResult> PerformanceResults { get; set; } = [];

        //public int? PerformanceId { get; set; }
    }
}
