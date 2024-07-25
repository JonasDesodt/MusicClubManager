using Microsoft.EntityFrameworkCore;
using MusicClubManager.Abstractions;
using MusicClubManager.Core;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Models;
using MusicClubManager.Services.Extensions.Filters;
using System.Linq;

namespace MusicClubManager.Services
{
    public class PerformanceDbService(MusicClubManagerDbContext dbContext) : IPerformanceService
    {


        public async Task<ServiceResult<PerformanceResult>> Create(PerformanceRequest request)
        {
            if (await dbContext.Artists.Include(a => a.Image).FirstOrDefaultAsync(a => a.Id == request.ArtistId) is not { } artist)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The performance is not created. The artist could not be found." }
                    ]
                };
            }

            if (await dbContext.Lineups.Include(l => l.Event).FirstOrDefaultAsync(l => l.Id == request.LineupId) is not { } lineup)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The performance is not created. The lineup could not be found." }
                    ]
                };
            }

            if (lineup.Doors > request.Start)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The performance is not created. The performance start is earlier than the lineup doors." }
                    ]
                };
            }

            var now = DateTime.UtcNow;

            var performance = new Performance
            {
                Duration = request.Duration,
                Start = request.Start,
                Type = request.Type,
                Created = now,
                Updated = now,
                ArtistId = request.ArtistId,
                LineupId = request.LineupId,
            };

            await dbContext.Performances.AddAsync(performance);

            await dbContext.SaveChangesAsync();

            return new ServiceResult<PerformanceResult>
            {
                Data = new PerformanceResult
                {
                    Id = performance.Id,
                    Duration = performance.Duration,
                    Start = performance.Start,
                    Type = performance.Type,
                    ArtistResult = new ArtistResult
                    {
                        Id = artist.Id,
                        Description = artist.Description,
                        Name = artist.Name,
                        ImageResult = artist.Image != null
                                    ? new ImageResult
                                    {
                                        Alt = artist.Image.Alt,
                                        ContentType = artist.Image.ContentType,
                                        Created = artist.Image.Created,
                                        Id = artist.Image.Id,
                                        Updated = artist.Image.Updated
                                    }
                                    : null
                    },
                    LineupResult = new PerformanceLineupResult
                    {
                        Id = lineup.Id,
                        Doors = lineup.Doors,
                        Name = lineup.Name,
                        IsSoldOut = true,
                        Event = lineup.Event != null
                        ? new EventResult
                        {
                            Id = lineup.Event.Id,
                            Name = lineup.Event.Name,
                        }
                        : null
                    }
                }
            };
        }

        public async Task<ServiceResult<PerformanceResult>> Delete(int id)
        {
            var performance = await dbContext.Performances.FindAsync(id);

            if (performance is null)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The performance has not been deleted. The performance could not be found." }
                    ]
                };
            }

            dbContext.Performances.Remove(performance);

            await dbContext.SaveChangesAsync();

            return new ServiceResult<PerformanceResult> { };
        }

        public async Task<ServiceResult<PerformanceResult>> Get(int id)
        {
            var performance = await dbContext.Performances
                .Include(p => p.Artist)
                .Include(p => p.Lineup)
                .ThenInclude(l => l != null ? l.Event : null)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (performance is null)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The performance could not be found." }
                    ]
                };
            }

            if (performance.Artist is null)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The artist could not be found." }
                    ]
                };
            }

            if (performance.Lineup is null)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The lineup could not be found." }
                    ]
                };
            }

            return new ServiceResult<PerformanceResult>
            {
                Data = new PerformanceResult
                {
                    Id = performance.Id,
                    Duration = performance.Duration,
                    Start = performance.Start,
                    ArtistResult = new ArtistResult
                    {
                        Id = performance.Artist.Id,
                        Name = performance.Artist.Name,
                        Description = performance.Artist.Description,
                        ImageResult = performance.Artist.Image != null
                                    ? new ImageResult
                                    {
                                        Alt = performance.Artist.Image.Alt,
                                        ContentType = performance.Artist.Image.ContentType,
                                        Created = performance.Artist.Image.Created,
                                        Id = performance.Artist.Image.Id,
                                        Updated = performance.Artist.Image.Updated
                                    }
                                    : null
                    },
                    LineupResult = new PerformanceLineupResult
                    {
                        Id = performance.Lineup.Id,
                        Doors = performance.Lineup.Doors,
                        IsSoldOut = performance.Lineup.IsSoldOut,
                        Name = performance.Lineup.Name,
                        Event = performance.Lineup.Event != null
                        ? new EventResult
                        {
                            Id = performance.Lineup.Event.Id,
                            Name = performance.Lineup.Event.Name
                        }
                        : null
                    }

                }
            };
        }

        /// <summary>
        /// id == LineupId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paginationRequest"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<PagedServiceResult<IList<LineupPerformanceResult>>> GetAll(int id, PaginationRequest paginationRequest, PerformanceFilter filter)
        {
            var skip = (paginationRequest.Page - 1) * paginationRequest.PageSize;

            var query = from performance in dbContext.Performances
                        join artist in dbContext.Artists on performance.ArtistId equals artist.Id
                        join image in dbContext.Images on artist.ImageId equals image.Id
                        where performance.LineupId == id
                        select new LineupPerformanceResult
                        {
                            Id = performance.Id,
                            Start = performance.Start,
                            //Duration = performance.Duration, => bugged: bigint in db 
                            Type = performance.Type,
                            ArtistResult = new ArtistResult
                            {
                                Id = artist.Id,
                                Name = artist.Name,
                                Description = artist.Description,
                                ImageResult = artist.Image != null 
                                ? new ImageResult
                                {
                                    Alt = artist.Image.Alt,
                                    ContentType = artist.Image.ContentType,
                                    Created = artist.Image.Created,
                                    Updated = artist.Image.Updated,
                                    Id = artist.Image.Id,

                                }
                                : null
                            }
                        };

            var totalCount = await query
                .AddFilter(filter)
                .CountAsync();

            var performanceResults = await query
                .AddFilter(filter)
                .Skip((int)skip)
                .Take((int)paginationRequest.PageSize)
                .ToListAsync();

            return new PagedServiceResult<IList<LineupPerformanceResult>>
            {
                TotalCount = (uint)totalCount,
                Page = paginationRequest.Page,
                PageSize = paginationRequest.PageSize,
                Data = performanceResults
            };
        }

        public async Task<PagedServiceResult<IList<PerformanceResult>>> GetAll(PaginationRequest paginationRequest, PerformanceFilter filter)
        {
            //var totalCount = await dbContext.Performances
            //    .AddFilter(filter)
            //    .CountAsync();

            var skip = (paginationRequest.Page - 1) * paginationRequest.PageSize;

            var query = from performance in dbContext.Performances
                        join lineup in dbContext.Lineups on performance.LineupId equals lineup.Id
                        join artist in dbContext.Artists on performance.ArtistId equals artist.Id
                        join image in dbContext.Images on artist.ImageId equals image.Id
                        select new PerformanceResult
                        {
                            Id = performance.Id,
                            Start = performance.Start,
                            //Duration = performance.Duration, => bugged: bigint in db 
                            Type = performance.Type,
                            ArtistResult = new ArtistResult
                            {
                                Id = artist.Id,
                                Name = artist.Name,
                                Description = artist.Description,
                                ImageResult = artist.Image != null
                                ? new ImageResult
                                {
                                    Alt = artist.Image.Alt,
                                    ContentType = artist.Image.ContentType,
                                    Created = artist.Image.Created,
                                    Updated = artist.Image.Updated,
                                    Id = artist.Image.Id,

                                }
                                : null
                            },
                            LineupResult = new PerformanceLineupResult
                            {
                                Id = lineup.Id,
                                Name = lineup.Name,
                                Doors = lineup.Doors,
                                IsSoldOut = lineup.IsSoldOut,
                                Event = null //temp 
                            }
                        };

            var totalCount = await query
                .AddFilter(filter)
                .CountAsync();

            var performanceResults = await query
                .AddFilter(filter)
                .Skip((int)skip)
                .Take((int)paginationRequest.PageSize)
                .ToListAsync();

            return new PagedServiceResult<IList<PerformanceResult>>
            {
                Data = performanceResults,
                TotalCount = (uint)totalCount,
                PageSize = paginationRequest.PageSize,
                Page = paginationRequest.Page,
            };
        }

        public async Task<ServiceResult<PerformanceResult>> Update(int id, PerformanceRequest request)
        {
            var performance = await dbContext.Performances.FindAsync(id);

            if (performance is null)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The performance has not been updated. The performance could not be found." }
                    ]
                };
            }

            if (await dbContext.Artists.FindAsync(request.ArtistId) is not { } artist)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The performance has not been updated. The artist could not be found." }
                    ]
                };
            }

            if (await dbContext.Lineups.Include(l => l.Event).FirstOrDefaultAsync(l => l.Id == request.LineupId) is not { } lineup)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The performance has not been updated. The lineup could not be found." }
                    ]
                };
            }

            if (lineup.Doors > request.Start)
            {
                return new ServiceResult<PerformanceResult>
                {
                    Messages =
                    [
                        new () { Message = "The performance is not created. The performance start is earlier than the lineup doors." }
                    ]
                };
            }

            performance.Duration = request.Duration;
            performance.Start = request.Start;
            performance.ArtistId = request.ArtistId;
            performance.LineupId = request.LineupId;
            performance.Updated = DateTime.UtcNow;

            dbContext.Lineups.Update(lineup);

            await dbContext.SaveChangesAsync();

            return new ServiceResult<PerformanceResult>
            {
                Data = new PerformanceResult
                {
                    Id = lineup.Id,
                    Duration = performance.Duration,
                    Start = performance.Start,
                    ArtistResult = new ArtistResult
                    {
                        Id = artist.Id,
                        Name = artist.Name,
                        Description = artist.Description,
                        ImageResult = null!, //TODO TEMP HACK
                    },
                    LineupResult = new PerformanceLineupResult
                    {
                        Id = lineup.Id,
                        Doors = lineup.Doors,
                        IsSoldOut = lineup.IsSoldOut,
                        Name = lineup.Name,
                        Event = lineup.Event != null
                        ? new EventResult
                        {
                            Id = lineup.Event.Id,
                            Name = lineup.Event.Name
                        }
                        : null
                    }
                }
            };
        }
    }
}