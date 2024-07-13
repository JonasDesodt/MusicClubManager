using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Services.Models
{
    internal class PerformanceTest
    {
        public required int Id { get; set; }

        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }
    }
}
