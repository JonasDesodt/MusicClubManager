using MusicClubManager.Dto.Enums;
using MusicClubManager.Dto.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Sdk.Extensions
{
    internal static class LineupFilterExtensions
    {
        public static string ToQueryString(this LineupFilter lineupFilter)
        {
            var builder = new StringBuilder();

            if (lineupFilter.Scoped) {
                builder.Append($"scoped=true&");
            } else
            {
                builder.Append($"scoped=false&");
            }

            if(lineupFilter.ArtistId is { } artistId && artistId > 0)
            {
                builder.Append($"artistId={lineupFilter.ArtistId}&");

            }

            if (!string.IsNullOrWhiteSpace(lineupFilter.Name))
            {
                builder.Append($"name={lineupFilter.Name}&");
            }

            if (!string.IsNullOrWhiteSpace(lineupFilter.SortProperty))
            {
                builder.Append($"sortProperty={lineupFilter.SortProperty}&");

                if (lineupFilter.SortDirection is SortDirection.Descending)
                {
                    builder.Append($"sortDirection={lineupFilter.SortDirection}&");
                }
            }

            return builder.ToString();
        }
    }
}
