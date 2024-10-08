﻿using MusicClubManager.Dto.Enums;
using MusicClubManager.Dto.Filters;
using System.Text;

namespace MusicClubManager.Sdk.Extensions
{
    internal static class PerformanceFilterExtensions
    {
        public static string ToQueryString(this PerformanceFilter performanceFilter)
        {
            var builder = new StringBuilder();

            if (performanceFilter.Id is not null || performanceFilter.Id > 0)
            {
                builder.Append($"id={performanceFilter.Id}&");
            }

            if (!string.IsNullOrWhiteSpace(performanceFilter.SortProperty))
            {
                builder.Append($"sortProperty={performanceFilter.SortProperty}&");

                if (performanceFilter.SortDirection is SortDirection.Descending)
                {
                    builder.Append($"sortDirection={performanceFilter.SortDirection}&");
                }
            }

            if(performanceFilter.Year is not null)
            {
                builder.Append($"year={performanceFilter.Year}&");
            }

            if (performanceFilter.Month is not null)
            {
                builder.Append($"month={performanceFilter.Month}&");
            }

            return builder.ToString();
        }
    }
}
