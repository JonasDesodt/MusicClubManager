using MusicClubManager.Dto.Filters;

namespace MusicClubManager.Blazor.Models.Contexts
{
    public class ArtistContext
    {
        public int Id { get; set; }

        public LineupFilter LineupFilter { get; set;  } = new LineupFilter();

        public PaginationContext LineupPaginationContext { get; set; } = new PaginationContext { Page = 1, PageSize = 12 };

        public bool AreLineupFiltersOpen { get; set; } = false;
    }
}