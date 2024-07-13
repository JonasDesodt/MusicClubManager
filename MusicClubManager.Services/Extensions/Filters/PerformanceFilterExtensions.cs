using MusicClubManager.Dto.Enums;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Result;
using MusicClubManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Services.Extensions.Filters
{
    public static class PerformanceFilterExtensions
    {
        public static IQueryable<LineupPerformanceResult> AddFilter(this IQueryable<LineupPerformanceResult> performances, PerformanceFilter performanceFilter)
        {
            //if (!string.IsNullOrWhiteSpace(filter.Name))
            //{
            //    performances = performances.Where(a => a.Name.ToLower().Contains(filter.Name.Trim().ToLower()));
            //}

            if (!string.IsNullOrWhiteSpace(performanceFilter.SortProperty))
            {
                if (performanceFilter.SortDirection is SortDirection.Descending)
                {
                    performances = performanceFilter.SortProperty switch
                    {
                        nameof(LineupPerformanceResult.ArtistResult) => performances.OrderByDescending(p => p.ArtistResult.Name),
                        _ => performances.OrderByDescending(p => p.Id),
                    };
                }
                else
                {
                    performances = performanceFilter.SortProperty switch
                    {
                        nameof(PerformanceResult.ArtistResult) => performances.OrderBy(p => p.ArtistResult.Name),
                        _ => performances.OrderBy(a => a.Id),
                    };
                }
            }

            return performances;
        }

        public static IQueryable<PerformanceResult> AddFilter(this IQueryable<PerformanceResult> performances, PerformanceFilter performanceFilter)
        {
            //if (!string.IsNullOrWhiteSpace(filter.Name))
            //{
            //    performances = performances.Where(a => a.Name.ToLower().Contains(filter.Name.Trim().ToLower()));
            //}

            if (!string.IsNullOrWhiteSpace(performanceFilter.SortProperty))
            {
                if (performanceFilter.SortDirection is SortDirection.Descending)
                {
                    performances = performanceFilter.SortProperty switch
                    {
                        nameof(PerformanceResult.ArtistResult) => performances.OrderByDescending(p => p.ArtistResult.Name), 
                        nameof(PerformanceResult.LineupResult) => performances.OrderByDescending(p => p.LineupResult.Name), 
                        _ => performances.OrderByDescending(p => p.Id),
                    };
                }
                else
                {
                    performances = performanceFilter.SortProperty switch
                    {
                        nameof(PerformanceResult.ArtistResult) => performances.OrderBy(p => p.ArtistResult.Name), 
                        nameof(PerformanceResult.LineupResult) => performances.OrderBy(p => p.LineupResult.Name), 
                        _ => performances.OrderBy(a => a.Id),
                    };
                }
            }

            return performances;
        }
    }
}
