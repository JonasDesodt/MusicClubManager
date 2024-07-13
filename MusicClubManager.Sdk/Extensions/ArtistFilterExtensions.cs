using MusicClubManager.Dto.Enums;
using MusicClubManager.Dto.Filters;
using System.Text;

namespace MusicClubManager.Sdk.Extensions
{
    internal static class ArtistFilterExtensions
    {
        public static string ToQueryString(this ArtistFilter artistFilter)
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(artistFilter.Name))
            {
                builder.Append($"name={artistFilter.Name}&");
            }

            if (!string.IsNullOrWhiteSpace(artistFilter.SortProperty))
            {
                builder.Append($"sortProperty={artistFilter.SortProperty}&");

                if (artistFilter.SortDirection is SortDirection.Descending)
                {
                    builder.Append($"sortDirection={artistFilter.SortDirection}&");
                }
            }

            return builder.ToString();
        }
    }
}