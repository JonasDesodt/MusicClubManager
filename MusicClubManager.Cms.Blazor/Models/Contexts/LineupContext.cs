using MusicClubManager.Dto.Filters;

namespace MusicClubManager.Cms.Blazor.Models.Contexts
{
    public class LineupContext
    {
        public LineupFilter LineupFilter { get; set; } = new();

        public Pagination Pagination { get; set; } = new ()
        {
            Page = 1,
            PageSize = 24
        };
    }
}
