using MusicClubManager.Dto.Enums;

namespace MusicClubManager.Dto.Filters
{
    public class PerformanceFilter
    {
        public int? Id { get; set; }

        public string? SortProperty { get; set; }

        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        public int? Year { get; set; }

        public int? Month { get; set; }
    }
}