using Microsoft.EntityFrameworkCore;
using MusicClubManager.Abstractions;
using MusicClubManager.Core;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Models;
using MusicClubManager.Services.Extensions;
using MusicClubManager.Services.Extensions.Filters;


namespace MusicClubManager.Services
{
    public class LineupDbService(MusicClubManagerDbContext dbContext) : ILineupService
    {
        public async Task<ServiceResult<LineupResult>> Create(LineupRequest request)
        {
            var now = DateTime.UtcNow;

            if (request.EventId is { } eventId)
            {
                if (await dbContext.Events.FindAsync(eventId) is null)
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

            if (request.ImageId is { } imageId)
            {
                if (await dbContext.Images.FindAsync(imageId) is null)
                {
                    return new ServiceResult<LineupResult>
                    {
                        Messages =
                        [
                            new () { Message = "The image can not be found."}
                        ]
                    };
                }
            }

            var lineup = new Lineup
            {
                Doors = request.Doors,
                Name = request.Name,
                IsSoldOut = request.IsSoldOut,
                EventId = request.EventId,
                ImageId = request.ImageId,
                Created = now,
                Updated = now
            };

            await dbContext.Lineups.AddAsync(lineup);

            await dbContext.SaveChangesAsync();

            lineup = await dbContext.Lineups.IncludeAll().FirstOrDefaultAsync();

            if (lineup is null)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages =
                    [
                        new () { Message = "The lineup is not created." }
                    ]
                };
            }

            return new ServiceResult<LineupResult>
            {
                Data = lineup.ToLineupResult()
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
            var lineup = await dbContext.Lineups
                .IncludeAll()
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
                Data = lineup.ToLineupResult()

            };
        }

        public async Task<ServiceResult<LineupResult>> Previous(int id)
        {
            var previous = await dbContext.Lineups.PreviousOrLast(id);

            if (previous is null)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages =
                    [
                        new () { Message = "The lineup could not be found." }
                    ]
                };
            }

            var lineupResult = previous.ToLineupResult();

            return new ServiceResult<LineupResult>
            {
                Data = lineupResult
            };
        }

        public async Task<ServiceResult<LineupResult>> Next(int id)
        {
            var next = await dbContext.Lineups.NextOrFirst(id);

            if (next is null)
            {
                return new ServiceResult<LineupResult>
                {
                    Messages =
                    [
                        new () { Message = "The lineup could not be found." }
                    ]
                };
            }

            var lineupResult = next.ToLineupResult();

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
                .Sort()
                .IncludeAll()
                .Select(l => l)
                .AddFilter(filter);

            var totalCount = await lineups.CountAsync();

            var skip = (paginationRequest.Page - 1) * paginationRequest.PageSize;

            var lineupResults = await lineups
                .Skip((int)skip)
                .Take((int)paginationRequest.PageSize)
                .Select(l => l.ToLineupResult()).ToListAsync();

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
                .IncludeAll()
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

            if (request.ImageId is not null)
            {
                lineup.Image = await dbContext.Images.FindAsync(request.ImageId);

                if (lineup.Image is null)
                {
                    return new ServiceResult<LineupResult>
                    {
                        Messages =
                        [
                            new () { Message = "The lineup has not been updated. The image could not be found." }
                        ]
                    };
                }
            }

            lineup.Doors = request.Doors;
            lineup.IsSoldOut = request.IsSoldOut;
            lineup.Name = request.Name;
            lineup.Updated = DateTime.UtcNow;
            lineup.ImageId = request.ImageId;
            lineup.EventId = request.EventId;

            await dbContext.SaveChangesAsync();

            return new ServiceResult<LineupResult>
            {
                Data = lineup.ToLineupResult()
            };
        }
    }

}