using MusicClubManager.Dto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Dto.Filters
{
    public class PerformanceFilter
    {
        public int? Id { get; set; }

        public string? SortProperty { get; set; }

        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    }
}
