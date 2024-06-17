using MusicClubManager.Dto.Filters;
using MusicClubManager.Models;

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

            return artists;
        }
    }
}