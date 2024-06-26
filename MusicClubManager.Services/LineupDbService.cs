﻿using Microsoft.EntityFrameworkCore;
using MusicClubManager.Abstractions;
using MusicClubManager.Core;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Models;
using MusicClubManager.Services.Extensions.Filters;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MusicClubManager.Services
{
    public class LineupDbService(MusicClubManagerDbContext dbContext) : ILineupService
    {
        public async Task<ServiceResult<LineupResult>> Create(LineupRequest request)
        {
            var lineup = new Lineup
            {
                Doors = request.Doors,
                Name = request.Name,
                IsSoldOut = request.IsSoldOut,
                EventId = request.EventId,
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

            if (await dbContext.Performances.AnyAsync(p => p.LineupId == id))
            {
                return new ServiceResult<LineupResult>
                {
                    Messages =
                    [
                        new () { Message = "The lineup has not been deleted. There are performances associated with this lineup." }
                    ]
                };
            }

            dbContext.Lineups.Remove(lineup);

            await dbContext.SaveChangesAsync();

            return new ServiceResult<LineupResult> { };
        }

        public async Task<ServiceResult<LineupResult>> Get(int id)
        {
            var performancesTotalCount = await dbContext.Performances.CountAsync(p => p.LineupId == id);

            var lineup = await dbContext.Lineups
                .Include(l => l.Event)
                .Include(l => (l.Performances.AsQueryable().Skip(0).Take(5).Include(p => p.Artist)))
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
                            Artist = p.Artist != null
                            ? new ArtistResult
                            {
                                Id = p.Artist.Id,
                                Name = p.Artist.Name,
                                Description = p.Artist.Description,
                                Image = p.Artist.Image
                            }
                            : null
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
                            .Include(p => p.Artist)
                            .Where(p => p.LineupId == id)
                            .Skip((int)skip)
                            .Take(take)
                            .Select(p => new LineupPerformanceResult
                            {
                                Id = p.Id,
                                Artist = p.Artist != null ?
                                            new ArtistResult
                                            { Id = p.Artist.Id, 
                                                Description = p.Artist.Description,
                                                Image = p.Artist.Image, 
                                                Name = p.Artist.Name }
                                            : null
                            })
                            .ToListAsync();

            return new ServiceResult<LineupResult>
            {
                Data = lineupResult
            };
        }

        public async Task<PagedServiceResult<IList<LineupResult>>> GetAll(PaginationRequest paginationRequest, LineupFilter filter)
        {
            var totalCount = await dbContext.Lineups
                .AddFilter(filter)
                .CountAsync();

            var skip = (paginationRequest.Page - 1) * paginationRequest.PageSize;

            // Retrieve the filtered, paginated lineups first
            var lineups = await dbContext.Lineups
                .AddFilter(filter)
                .Skip((int)skip)
                .Take((int)paginationRequest.PageSize)
                .Include(l => l.Event)
                .ToListAsync();

            // Prepare the result list
            var lineupResults = lineups.Select(l => new LineupResult
            {
                Id = l.Id,
                Doors = l.Doors,
                Name = l.Name,
                IsSoldOut = l.IsSoldOut,
                Event = l.Event != null
                    ? new EventResult
                    {
                        Id = l.Event.Id,
                        Name = l.Event.Name,
                    }
                    : null,
                PagedLineupPerformanceResult = new PagedResult<IList<LineupPerformanceResult>>
                {
                    Data = [],
                    Page = paginationRequest.Page,
                    PageSize = paginationRequest.PageSize,
                    TotalCount = 100
                }
            }).ToList();

            // Load performances separately for each lineup
            foreach (var lineup in lineupResults)
            {
                var performancesTotalCount = await dbContext.Performances.CountAsync(p => p.LineupId == lineup.Id);

                var performances = await dbContext.Performances
                    .Where(p => p.LineupId == lineup.Id)
                    .Include(p => p.Artist)
                    .Take(5)
                    .ToListAsync();

                lineup.PagedLineupPerformanceResult = new PagedResult<IList<LineupPerformanceResult>>
                {
                    Data = performances.Select(p => new LineupPerformanceResult
                    {
                        Id = p.Id,
                        Artist = p.Artist != null
                            ? new ArtistResult
                            {
                                Id = p.Artist.Id,
                                Name = p.Artist.Name,
                                Description = p.Artist.Description,
                                Image = p.Artist.Image
                            }
                            : null
                    }).ToList(),
                    Page = 1,
                    PageSize = 5,
                    TotalCount = (uint)performancesTotalCount
                };
            }

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
            var lineup = await dbContext.Lineups
                .Include(l => (l.Performances.AsQueryable().Skip(0).Take(5).Include(p => p.Artist)))
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lineup is null)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages =
                    [
                        new () { Message = "The lineup has not been updated. The lineup could not be found." }
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

            lineup.Name = request.Name;
            lineup.IsSoldOut = request.IsSoldOut;
            lineup.Doors = request.Doors;
            lineup.EventId = request.EventId;

            dbContext.Lineups.Update(lineup);

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
                    PagedLineupPerformanceResult = new PagedResult<IList<LineupPerformanceResult>>
                    {
                        Data = lineup.Performances.Select(p => new LineupPerformanceResult
                        {
                            Id = p.Id,
                            Artist = p.Artist != null
                            ? new ArtistResult
                            {
                                Id = p.Artist.Id,
                                Name = p.Artist.Name,
                                Description = p.Artist.Description,
                                Image = p.Artist.Image
                            }
                            : null
                        }).ToList(),
                        Page = 1,
                        PageSize = 5,
                        TotalCount = (uint)lineup.Performances.Count // does this counts the performances after the skip/take or does it return the totalCount?
                    }
                }
            };
        }
    }
}