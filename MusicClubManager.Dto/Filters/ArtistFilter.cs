using MusicClubManager.Dto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Dto.Filters
{
    public class ArtistFilter
    {
        public string? Name { get; set; }          

        public string? SortProperty { get; set; }

        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    }
}
