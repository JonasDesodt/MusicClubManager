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
using System.Collections.Generic;


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
                ImageId = request.ImageId,
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
                //.Sort()
                .Include(l => l.Event)
                .Include(l => l.Image)
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
                    IsSoldOut = lineup.IsSoldOut,
                    ImageResult = lineup.Image == null ? null : new ImageResult { Alt = lineup.Image.Alt, ContentType = lineup.Image.ContentType, Created = lineup.Image.Created, Id = lineup.Image.Id, Updated = lineup.Image.Updated },
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
                            BandcampId = p.BandcampId,
                            BandcampLink = p.BandcampLink,
                            Description = p.Description,
                            Name = p.Name,
                            Spotify = p.Spotify,
                            YouTube = p.YouTube,
                            Duration = p.Duration,
                            Start = p.Start,
                            Type = p.Type,
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
                //.Sort()
                .Include(l => l.Event)
                .Include(l => l.Image)
                .Select(l => new LineupResult
                {
                    Doors = l.Doors,
                    Event = l.Event != null ? new EventResult { Id = l.Event.Id, Name = l.Event.Name } : null,
                    ImageResult = l.Image == null ? null : new ImageResult { Alt = l.Image.Alt, ContentType = l.Image.ContentType, Created = l.Image.Created, Id = l.Image.Id, Updated = l.Image.Updated },
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
                                BandcampId = p.BandcampId,
                                BandcampLink = p.BandcampLink,
                                Description = p.Description,
                                Name = p.Name,
                                Spotify = p.Spotify,
                                YouTube = p.YouTube,
                                Duration = p.Duration,
                                Start = p.Start,
                                Type = p.Type,
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

        public async Task<ServiceResult<LineupResult>> Previous(int id, PaginationRequest paginationRequest)
        {
            var performancesTotalCount = await dbContext.Performances.CountAsync(p => p.LineupId == id);

            uint skip = (paginationRequest.Page - 1) * paginationRequest.PageSize; ;
            int take = 5;


            //using (var context = dbContext)
            //{
            //    var previous = context.Lineups
            //                         .FromSqlInterpolated($"SELECT *\r\nFROM lineups AS t\r\nWHERE t.Id = (\r\n    SELECT \r\n        subquery.Previous\r\n    FROM (\r\n        SELECT \r\n            t.*, \r\n            LAG(t.Id) OVER (ORDER BY t.Doors, t.Name, t.Id) AS Previous, \r\n            LEAD(t.Id) OVER (ORDER BY t.Doors, t.Name, t.Id) AS Next\r\n        FROM \r\n            lineups AS t\r\n    ) AS subquery\r\n    WHERE subquery.Id = {id}\r\n)")
            //                         .FirstOrDefault();
            //}

            using var context = dbContext;
          
            var previous = context.Lineups.FromSqlInterpolated($"SELECT *\r\nFROM lineups AS t\r\nWHERE t.Id = (SELECT subquery.Previous FROM ( SELECT t.*, LAG(t.Id) OVER (ORDER BY t.Doors, t.Name, t.Created, t.Updated, t.Id) AS Previous FROM lineups AS t) AS subquery WHERE subquery.Id = {id})").FirstOrDefault();
            if (previous is null)
            {
                var last = await dbContext.Lineups
                .Sort()
                .Include(l => l.Event)
                .Include(l => l.Image)
                .Select(l => new LineupResult
                {
                    Doors = l.Doors,
                    Event = l.Event != null ? new EventResult { Id = l.Event.Id, Name = l.Event.Name } : null,
                    ImageResult = l.Image == null ? null : new ImageResult { Alt = l.Image.Alt, ContentType = l.Image.ContentType, Created = l.Image.Created, Id = l.Image.Id, Updated = l.Image.Updated },
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
                .LastOrDefaultAsync();

                if (last is null)
                {
                    return new ServiceResult<LineupResult>
                    {

                    };
                }

                last.PagedLineupPerformanceResult.Data = await dbContext.Performances
                                .Include(p => p.Artist).ThenInclude(a => a != null ? a.Image : null)
                                .Where(p => p.LineupId == id)
                                .Skip((int)skip)
                                .Take(take)
                                .Select(p => new LineupPerformanceResult
                                {
                                    Id = p.Id,
                                    BandcampId = p.BandcampId,
                                    BandcampLink = p.BandcampLink,
                                    Description = p.Description,
                                    Name = p.Name,
                                    Spotify = p.Spotify,
                                    YouTube = p.YouTube,
                                    Duration = p.Duration,
                                    Start = p.Start,
                                    Type = p.Type,
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
                    Data = last
                };
            }

            //todo: do this in the raw query
            previous.Image = await dbContext.Images.FindAsync(previous.ImageId);

            var lineupResult = new LineupResult
            {
                Doors = previous.Doors,
                //Event = l.Event != null ? new EventResult { Id = l.Event.Id, Name = l.Event.Name } : null,
                ImageResult = previous.Image == null ? null : new ImageResult { Alt = previous.Image.Alt, ContentType = previous.Image.ContentType, Created = previous.Image.Created, Id = previous.Image.Id, Updated = previous.Image.Updated },
                Id = previous.Id,
                Name = previous.Name,
                IsSoldOut = previous.IsSoldOut,
                PagedLineupPerformanceResult = new PagedResult<IList<LineupPerformanceResult>>
                {
                    Page = paginationRequest.Page,
                    PageSize = paginationRequest.PageSize,
                    TotalCount = (uint)performancesTotalCount
                }
            };

            lineupResult.PagedLineupPerformanceResult.Data = await dbContext.Performances
                              .Include(p => p.Artist).ThenInclude(a => a != null ? a.Image : null)
                              .Where(p => p.LineupId == id)
                              .Skip((int)skip)
                              .Take(take)
                              .Select(p => new LineupPerformanceResult
                              {
                                  Id = p.Id,
                                  BandcampId = p.BandcampId,
                                  BandcampLink = p.BandcampLink,
                                  Description = p.Description,
                                  Name = p.Name,
                                  Spotify = p.Spotify,
                                  YouTube = p.YouTube,
                                  Duration = p.Duration,
                                  Start = p.Start,
                                  Type = p.Type,
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

        public async Task<ServiceResult<LineupResult>> Next(int id, PaginationRequest paginationRequest)
        {
            var performancesTotalCount = await dbContext.Performances.CountAsync(p => p.LineupId == id);

            uint skip = (paginationRequest.Page - 1) * paginationRequest.PageSize; ;
            int take = 5;


            //using (var context = dbContext)
            //{
            //    var previous = context.Lineups
            //                         .FromSqlInterpolated($"SELECT *\r\nFROM lineups AS t\r\nWHERE t.Id = (\r\n    SELECT \r\n        subquery.Previous\r\n    FROM (\r\n        SELECT \r\n            t.*, \r\n            LAG(t.Id) OVER (ORDER BY t.Doors, t.Name, t.Id) AS Previous, \r\n            LEAD(t.Id) OVER (ORDER BY t.Doors, t.Name, t.Id) AS Next\r\n        FROM \r\n            lineups AS t\r\n    ) AS subquery\r\n    WHERE subquery.Id = {id}\r\n)")
            //                         .FirstOrDefault();
            //}

            using var context = dbContext;

            var previous = context.Lineups.FromSqlInterpolated($"SELECT *\r\nFROM lineups AS t\r\nWHERE t.Id = (SELECT subquery.Previous FROM ( SELECT t.*, LEAD(t.Id) OVER (ORDER BY t.Doors, t.Name, t.Created, t.Updated, t.Id) AS Previous FROM lineups AS t) AS subquery WHERE subquery.Id = {id})").FirstOrDefault();
            if (previous is null)
            {
                var first = await dbContext.Lineups
                .Sort()
                .Include(l => l.Event)
                .Include(l => l.Image)
                .Select(l => new LineupResult
                {
                    Doors = l.Doors,
                    Event = l.Event != null ? new EventResult { Id = l.Event.Id, Name = l.Event.Name } : null,
                    ImageResult = l.Image == null ? null : new ImageResult { Alt = l.Image.Alt, ContentType = l.Image.ContentType, Created = l.Image.Created, Id = l.Image.Id, Updated = l.Image.Updated },
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
                .FirstOrDefaultAsync();

                if (first is null)
                {
                    return new ServiceResult<LineupResult>
                    {

                    };
                }

                first.PagedLineupPerformanceResult.Data = await dbContext.Performances
                                .Include(p => p.Artist).ThenInclude(a => a != null ? a.Image : null)
                                .Where(p => p.LineupId == id)
                                .Skip((int)skip)
                                .Take(take)
                                .Select(p => new LineupPerformanceResult
                                {
                                    Id = p.Id,
                                    BandcampId = p.BandcampId,
                                    BandcampLink = p.BandcampLink,
                                    Description = p.Description,
                                    Name = p.Name,
                                    Spotify = p.Spotify,
                                    YouTube = p.YouTube,
                                    Duration = p.Duration,
                                    Start = p.Start,
                                    Type = p.Type,
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
                    Data = first
                };
            }

            //todo: do this in the raw query
            previous.Image = await dbContext.Images.FindAsync(previous.ImageId);

            var lineupResult = new LineupResult
            {
                Doors = previous.Doors,
                //Event = l.Event != null ? new EventResult { Id = l.Event.Id, Name = l.Event.Name } : null,
                ImageResult = previous.Image == null ? null : new ImageResult { Alt = previous.Image.Alt, ContentType = previous.Image.ContentType, Created = previous.Image.Created, Id = previous.Image.Id, Updated = previous.Image.Updated },
                Id = previous.Id,
                Name = previous.Name,
                IsSoldOut = previous.IsSoldOut,
                PagedLineupPerformanceResult = new PagedResult<IList<LineupPerformanceResult>>
                {
                    Page = paginationRequest.Page,
                    PageSize = paginationRequest.PageSize,
                    TotalCount = (uint)performancesTotalCount
                }
            };

            lineupResult.PagedLineupPerformanceResult.Data = await dbContext.Performances
                              .Include(p => p.Artist).ThenInclude(a => a != null ? a.Image : null)
                              .Where(p => p.LineupId == id)
                              .Skip((int)skip)
                              .Take(take)
                              .Select(p => new LineupPerformanceResult
                              {
                                  Id = p.Id,
                                  BandcampId = p.BandcampId,
                                  BandcampLink = p.BandcampLink,
                                  Description = p.Description,
                                  Name = p.Name,
                                  Spotify = p.Spotify,
                                  YouTube = p.YouTube,
                                  Duration = p.Duration,
                                  Start = p.Start,
                                  Type = p.Type,
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
                .Sort()
                .Include(l => l.Image)
                .Include(l => l.Performances)
                .ThenInclude(p => p.Artist).ThenInclude(a => a != null ? a.Image : null)
                .Select(l => l)
                .AddFilter(filter);

            var totalCount = await lineups.CountAsync();

            var skip = (paginationRequest.Page - 1) * paginationRequest.PageSize;

            var lineupResults = await lineups
                .Skip((int)skip)
                .Take((int)paginationRequest.PageSize)
                .Select(l => new LineupResult
                {
                    Doors = l.Doors,
                    Id = l.Id,
                    IsSoldOut = l.IsSoldOut,
                    Name = l.Name,
                    ImageResult = l.Image == null ? null : new ImageResult { Alt = l.Image.Alt, ContentType = l.Image.ContentType, Created = l.Image.Created, Id = l.Image.Id, Updated = l.Image.Updated },
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
                            Type = p.Type,
                            BandcampId = p.BandcampId,
                            BandcampLink = p.BandcampLink,
                            Description = p.Description,
                            Name = p.Name,
                            Spotify = p.Spotify,
                            YouTube = p.YouTube
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
            var lineup = await dbContext.Lineups
                .Include(l => l.Image)
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
                    ImageResult = lineup.Image == null ? null : new ImageResult { Alt = lineup.Image.Alt, ContentType = lineup.Image.ContentType, Created = lineup.Image.Created, Id = lineup.Image.Id, Updated = lineup.Image.Updated },
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