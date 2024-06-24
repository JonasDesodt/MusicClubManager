using MusicClubManager.Dto.Enums;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Result;
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


            if (!string.IsNullOrWhiteSpace(filter.SortProperty))
            {
                if (filter.SortDirection is SortDirection.Descending)
                {
                    lineups = filter.SortProperty switch
                    {
                        nameof(LineupResult.Name) => lineups.OrderByDescending(a => a.Name),
                        _ => lineups.OrderByDescending(a => a.Id),
                    };
                }
                else
                {
                    lineups = filter.SortProperty switch
                    {
                        nameof(LineupResult.Name) => lineups.OrderBy(a => a.Name),
                        _ => lineups.OrderBy(a => a.Id),
                    };
                }
            }


            return lineups;
        }
    }
}