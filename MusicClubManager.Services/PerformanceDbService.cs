using Microsoft.EntityFrameworkCore;
using MusicClubManager.Abstractions;
using MusicClubManager.Core;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Models;

namespace MusicClubManager.Services
{
    public class PerformanceDbService(MusicClubManagerDbContext dbContext) : IPerformanceService
    {
        public async Task<ServiceResult<PerformanceResult>> Create(PerformanceRequest request)
        {
            if (await dbContext.Artists.FirstOrDefaultAsync(a => a.Id == request.ArtistId) is not { } artist)
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

            var performance = new Performance
            {
                Duration = request.Duration,
                Start = request.Start,
                Type = request.Type,
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
                    Artist = new ArtistResult
                    {
                        Id = artist.Id,
                        Description = artist.Description,
                        Name = artist.Name,
                        Image = artist.Image
                    },
                    Lineup = new LineupResult
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

        public Task<ServiceResult<PerformanceResult>> Delete(int id)
        {
            throw new NotImplementedException();
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
                    Artist = new ArtistResult
                    {
                        Id = performance.Artist.Id,
                        Name = performance.Artist.Name,
                        Description = performance.Artist.Description,
                        Image = performance.Artist.Image
                    },
                    Lineup = new LineupResult
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

        public async Task<PagedServiceResult<IList<PerformanceResult>>> GetAll(PaginationRequest paginationRequest, PerformanceFilter filter)
        {
            var totalCount = await dbContext.Artists.CountAsync();

            var skip = (paginationRequest.Page - 1) * paginationRequest.PageSize;

            var performanceResults = await dbContext.Performances
                .Include(p => p.Artist)
                .Include(p => p.Lineup)
                .ThenInclude(l => l != null ? l.Event : null)
                .Skip((int)skip)
                .Take((int)paginationRequest.PageSize)
                .Select(p => 
                new PerformanceResult
                {
                    Id = p.Id,
                    Type = p.Type,
                    Duration = p.Duration,
                    Start = p.Start,
                    Artist = p.Artist != null
                    ? new ArtistResult
                    {
                        Id = p.Artist.Id,
                        Name = p.Artist.Name,
                        Image = p.Artist.Image,
                        Description = p.Artist.Description
                    }
                    : null,
                    Lineup = p.Lineup != null
                    ? new LineupResult
                    {
                        Id = p.Lineup.Id,
                        Doors = p.Lineup.Doors,
                        IsSoldOut = p.Lineup.IsSoldOut,
                        Name = p.Lineup.Name,
                        Event = p.Lineup.Event != null
                        ? new EventResult
                        {
                            Id = p.Lineup.Event.Id,
                            Name = p.Lineup.Event.Name,
                        }
                        : null
                    }
                    : null
                })
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

            dbContext.Lineups.Update(lineup);

            await dbContext.SaveChangesAsync();

            return new ServiceResult<PerformanceResult>
            {
                Data = new PerformanceResult
                {
                    Id = lineup.Id,
                    Duration = performance.Duration,
                    Start = performance.Start,
                    Artist = new ArtistResult
                    {
                        Id = artist.Id,
                        Name = artist.Name,
                        Description = artist.Description,
                        Image = artist.Image,
                    },
                    Lineup = new LineupResult
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