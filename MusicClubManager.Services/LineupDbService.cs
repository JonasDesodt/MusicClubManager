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
            var lineup = await dbContext.Lineups
                .Include(l => l.Event)
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
                    }
                }
            };
        }

        public async Task<PagedServiceResult<IList<LineupResult>>> GetAll(PaginationRequest paginationRequest, LineupFilter filter)
        {
            var totalCount = await dbContext.Lineups
                .AddFilter(filter)
                .CountAsync();

            var skip = (paginationRequest.Page - 1) * paginationRequest.PageSize;

            var lineupsResults = await dbContext.Lineups
                .AddFilter(filter)
                .Skip((int)skip)
                .Take((int)paginationRequest.PageSize)
                .Include(l => l.Event)
                .Select(l => new LineupResult
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
                    : null
                })
                .ToListAsync();

            return new PagedServiceResult<IList<LineupResult>>()
            {
                Data = lineupsResults,
                TotalCount = (uint)totalCount,
                PageSize = paginationRequest.PageSize,
                Page = paginationRequest.Page,
            };
        }

        public async Task<ServiceResult<LineupResult>> Update(int id, LineupRequest request)
        {
            var lineup = await dbContext.Lineups.FindAsync(id);

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
            
            if(request.EventId is not null)
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
                    : null
                }
            };
        }
    }
}