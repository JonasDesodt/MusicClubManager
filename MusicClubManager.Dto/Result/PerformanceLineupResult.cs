using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Dto.Result
{
    //should have a PerformanceResult as parent
    public class PerformanceLineupResult
    {
        public required int Id { get; set; }

        public string? Name { get; set; }
        public required DateTime Doors { get; set; }
        public bool IsSoldOut { get; set; }

        public EventResult? Event { get; set; }
    }
}
