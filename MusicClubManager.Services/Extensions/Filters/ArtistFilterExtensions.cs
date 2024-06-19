using MusicClubManager.Dto.Filters;
using MusicClubManager.Models;
using MusicClubManager.Dto.Result;

namespace MusicClubManager.Services.Extensions.Filters
{
    public static class ArtistFilterExtensions
    {
        public static IQueryable<Artist> AddFilter(this IQueryable<Artist> artists, ArtistFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                artists = artists.Where(a => a.Name.ToLower().Contains(filter.Name.Trim().ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.SortProperty))
            {
                artists = filter.SortProperty switch
                {
                    nameof(ArtistResult.Name) => artists.OrderBy(a => a.Name),
                    _ => artists.OrderBy(a => a.Id),
                };
            }

            return artists;
        }
    }
}