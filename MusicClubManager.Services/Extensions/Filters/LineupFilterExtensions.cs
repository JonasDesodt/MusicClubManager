using MusicClubManager.Dto.Filters;
using MusicClubManager.Models;

namespace MusicClubManager.Services.Extensions.Filters
{
    public static class LineupFilterExtensions
    {
        public static IQueryable<Lineup> AddFilter(this IQueryable<Lineup> lineups, LineupFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                lineups = lineups.Where(l => l.Name != null && l.Name.ToLower().Contains(filter.Name.Trim().ToLower()));
            }

            return lineups;
        }
    }
}