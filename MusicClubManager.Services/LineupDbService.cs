using Microsoft.EntityFrameworkCore;
using MusicClubManager.Abstractions;
using MusicClubManager.Core;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Models;
using MusicClubManager.Services.Extensions.Filters;


namespace MusicClubManager.Services
{
    public class LineupDbService(MusicClubManagerDbContext dbContext) : ILineupService
    {
        public async Task<ServiceResult<LineupResult>> Create(LineupRequest request)
        {
            var now = DateTime.UtcNow;

            var lineup = new Lineup
            {
                Doors = request.Doors,
                Name = request.Name,
                IsSoldOut = request.IsSoldOut,
                EventId = request.EventId,
                Created = now,
                Updated = now
            };

            await dbContext.Lineups.AddAsync(lineup);

            await dbContext.SaveChangesAsync();

            if (lineup.EventId is not null)
            {
                lineup.Event = await dbContext.Events.FindAsync(lineup.EventId);

                if (lineup.Event is null)
                {
                    return new ServiceResult<LineupResult>
                    {
                        Messages =
                        [
                            new () { Message = "The event can not be found."}
                        ]
                    };
                }
            }

            return new ServiceResult<LineupResult>
            {
                Data = new LineupResult
                {
                    Id = lineup.Id,
                    Doors = lineup.Doors,
                    Name = lineup.Name,
                    Event = lineup.Event == null ? null : new EventResult
                    {
                        Id = lineup.Event.Id,
                        Name = lineup.Event.Name,
                    },
                    IsSoldOut = lineup.IsSoldOut,
                    PagedLineupPerformanceResult = new PagedResult<IList<LineupPerformanceResult>>
                    {
                        Data = [],
                        Page = 1,
                        PageSize = 24,
                        TotalCount = 0
                    }
                }
            };
        }

        public async Task<ServiceResult<LineupResult>> Delete(int id)
        {
            var lineup = await dbContext.Lineups.FindAsync(id);

            if (lineup is null)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages =
                    [
                        new () { Message = "The lineup has not been deleted. The lineup could not be found." }
                    ]
                };
            }

            var performances = await dbContext.Performances.Where(p => p.LineupId == id).ToListAsync();

            foreach (var performance in performances)
            {
                dbContext.Remove(performance);
            }

            await dbContext.SaveChangesAsync();

            //if (await dbContext.Performances.AnyAsync(p => p.LineupId == id))
            //{
            //    return new ServiceResult<LineupResult>
            //    {
            //        Messages =
            //        [
            //            new () { Message = "The lineup has not been deleted. There are performances associated with this lineup." }
            //        ]
            //    };
            //}

            dbContext.Lineups.Remove(lineup);

            await dbContext.SaveChangesAsync();

            return new ServiceResult<LineupResult> { };
        }

        public async Task<ServiceResult<LineupResult>> Get(int id)
        {
            var performancesTotalCount = await dbContext.Performances.CountAsync(p => p.LineupId == id);

            var lineup = await dbContext.Lineups
                .Include(l => l.Event)

                .Include(l => (l.Performances.AsQueryable().Skip(0).Take(5).Include(p => p.Artist).ThenInclude(a => a != null ? a.Image : null)))
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lineup is null)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages =
                    [
                        new () { Message = "The lineup could not be found." }
                    ]
                };
            }

            return new ServiceResult<LineupResult>
            {
                Data = new LineupResult
                {
                    Id = lineup.Id,
                    Doors = lineup.Doors,
                    Name = lineup.Name,
                    Event = lineup.Event == null ? null : new EventResult
                    {
                        Id = lineup.Event.Id,
                        Name = lineup.Event.Name,
                    },
                    PagedLineupPerformanceResult = new PagedResult<IList<LineupPerformanceResult>>
                    {
                        Data = lineup.Performances.Select(p => new LineupPerformanceResult
                        {
                            Id = p.Id,
                            ArtistResult = p.Artist != null
                            ? new ArtistResult
                            {
                                Id = p.Artist.Id,
                                Name = p.Artist.Name,
                                Description = p.Artist.Description,
                                ImageResult = p.Artist.Image != null
                                ? new ImageResult
                                {
                                    Alt = p.Artist.Image.Alt,
                                    ContentType = p.Artist.Image.ContentType,
                                    Created = p.Artist.Image.Created,
                                    Id = p.Artist.Image.Id,
                                    Updated = p.Artist.Image.Updated
                                }
                                : null
                            }
                            : null! //temp hack, the artist in a performace should never be null
                        }).ToList(),
                        Page = 1,
                        PageSize = 5,
                        TotalCount = (uint)performancesTotalCount // does this counts the performances after the skip/take or does it return the totalCount?
                    }
                }
            };
        }



        public async Task<ServiceResult<LineupResult>> Get(int id, PaginationRequest paginationRequest)
        {
            var performancesTotalCount = await dbContext.Performances.CountAsync(p => p.LineupId == id);

            uint skip = (paginationRequest.Page - 1) * paginationRequest.PageSize; ;
            int take = 5;

            var lineupResult = await dbContext.Lineups
                .Include(l => l.Event)
                .Select(l => new LineupResult
                {
                    Doors = l.Doors,
                    Event = l.Event != null ? new EventResult { Id = l.Event.Id, Name = l.Event.Name } : null,
                    Id = l.Id,
                    Name = l.Name,
                    IsSoldOut = l.IsSoldOut,
                    PagedLineupPerformanceResult = new PagedResult<IList<LineupPerformanceResult>>
                    {
                        Page = paginationRequest.Page,
                        PageSize = paginationRequest.PageSize,
                        TotalCount = (uint)performancesTotalCount

                    }
                })
            .FirstOrDefaultAsync(l => l.Id == id);

            if (lineupResult is null)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages =
                    [
                        new () { Message = "The lineup could not be found." }
                    ]
                };
            }

            lineupResult.PagedLineupPerformanceResult.Data = await dbContext.Performances
                            .Include(p => p.Artist).ThenInclude(a => a != null ? a.Image : null)
                            .Where(p => p.LineupId == id)
                            .Skip((int)skip)
                            .Take(take)
                            .Select(p => new LineupPerformanceResult
                            {
                                Id = p.Id,
                                ArtistResult = p.Artist != null ?
                                            new ArtistResult
                                            {
                                                Id = p.Artist.Id,
                                                Description = p.Artist.Description,
                                                ImageResult = p.Artist.Image != null
                                                            ? new ImageResult
                                                            {
                                                                Alt = p.Artist.Image.Alt,
                                                                ContentType = p.Artist.Image.ContentType,
                                                                Created = p.Artist.Image.Created,
                                                                Id = p.Artist.Image.Id,
                                                                Updated = p.Artist.Image.Updated
                                                            }
                                                            : null,
                                                Name = p.Artist.Name
                                            }
                                            : null! //temp hack, the artist in a performace should never be null
                            })
                            .ToListAsync();

            return new ServiceResult<LineupResult>
            {
                Data = lineupResult
            };
        }

        public async Task<PagedServiceResult<IList<LineupResult>>> GetAll(PaginationRequest paginationRequest, LineupFilter filter)
        {
            if (filter.ArtistId is { } artistId && dbContext.Artists.Find(artistId) is null)
            {
                return new PagedServiceResult<IList<LineupResult>>()
                {
                    Messages = [new() { Message = $"The artist with id {filter.ArtistId} could not be found" }],
                    TotalCount = 0,
                    PageSize = paginationRequest.PageSize,
                    Page = paginationRequest.Page,
                };
            };

            var lineups = dbContext.Lineups
                .Include(l => l.Performances)
                .ThenInclude(p => p.Artist).ThenInclude(a => a != null ? a.Image : null)
                .Select(l => l)
                .AddFilter(filter);

            var totalCount = await lineups.CountAsync();

            var lineupResults = await lineups.Select(l => new LineupResult
            {
                Doors = l.Doors,
                Id = l.Id,
                IsSoldOut = l.IsSoldOut,
                Name = l.Name,
                PagedLineupPerformanceResult = new PagedResult<IList<LineupPerformanceResult>>
                {
                    PageSize = 5,
                    Page = 1,
                    TotalCount = (uint)l.Performances.Count(),
                    Data = l.Performances.Take(5).Select(p => new LineupPerformanceResult
                    {
                        ArtistResult = p.Artist != null ? new ArtistResult
                        {
                            Id = p.Artist.Id,
                            Name = p.Artist.Name,
                            Description = p.Artist.Description,
                            ImageResult = p.Artist.Image != null
                                                            ? new ImageResult
                                                            {
                                                                Alt = p.Artist.Image.Alt,
                                                                ContentType = p.Artist.Image.ContentType,
                                                                Created = p.Artist.Image.Created,
                                                                Id = p.Artist.Image.Id,
                                                                Updated = p.Artist.Image.Updated
                                                            }
                                                            : null
                        } : null!, //temp hack, the artist in a performace should never be null
                        Id = p.Id,
                        Duration = p.Duration,
                        Start = p.Start,
                        Type = p.Type
                    }).ToList()
                }
            }).ToListAsync();

            return new PagedServiceResult<IList<LineupResult>>()
            {
                Data = lineupResults,
                TotalCount = (uint)totalCount,
                PageSize = paginationRequest.PageSize,
                Page = paginationRequest.Page,
            };
        }


        public async Task<ServiceResult<LineupResult>> Update(int id, LineupRequest request)
        {
            var lineup = await dbContext.Lineups.FirstOrDefaultAsync(l => l.Id == id);
            if (lineup is null)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages =
                    [
                        new () { Message = "The lineup could not be found." }
                    ]
                };
            }

            if (request.EventId is not null)
            {
                lineup.Event = await dbContext.Events.FindAsync(request.EventId);

                if (lineup.Event is null)
                {
                    return new ServiceResult<LineupResult>
                    {
                        Messages =
                        [
                            new () { Message = "The lineup has not been updated. The event could not be found." }
                        ]
                    };
                }
            }

            lineup.Doors = request.Doors;
            lineup.IsSoldOut = request.IsSoldOut;
            lineup.Name = request.Name;
            lineup.Updated = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return new ServiceResult<LineupResult>
            {
                Data = new LineupResult
                {
                    Id = lineup.Id,
                    Name = lineup.Name,
                    Doors = lineup.Doors,
                    IsSoldOut = request.IsSoldOut,
                    Event = lineup.Event != null
                    ? new EventResult
                    {
                        Id = lineup.Event.Id,
                        Name = lineup.Event.Name
                    }
                    : null,
                    PagedLineupPerformanceResult = null! //TODO: temp hack
                }
            };
        }
    }

}