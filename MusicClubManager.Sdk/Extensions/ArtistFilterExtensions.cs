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
                builder.Append($"name={artistFilter.Name}");
            }

            return builder.ToString();
        }
    }
}