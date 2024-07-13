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
            if (filter.Scoped && filter.ArtistId is { } artistId && artistId > 0)
            {
                lineups = lineups.Where(l => l.Performances.Any(p => p.ArtistId == artistId));
            }

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

        public static IQueryable<LineupResult> AddFilter(this IQueryable<LineupResult> lineups, LineupFilter filter)
        {
            //if (filter.Scoped && filter.ArtistId is { } artistId && artistId > 0)
            //{
            //    lineups = lineups.Where(l => l.PagedLineupPerformanceResult.Data != null && l.PagedLineupPerformanceResult.Data.Any(p => p.Artist != null && p.Artist.Id == artistId));
            //}

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