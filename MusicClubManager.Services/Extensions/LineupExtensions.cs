using Microsoft.EntityFrameworkCore;
using MusicClubManager.Dto.Result;
using MusicClubManager.Models;

namespace MusicClubManager.Services.Extensions
{
    internal static class LineupExtensions
    {
        public static IQueryable<Lineup> Sort(this IQueryable<Lineup> lineups)
        {
            return lineups
                    .OrderBy(l => l.Doors)
                    .ThenBy(l => l.Name)
                    .ThenBy(l => l.Created)
                    .ThenBy(l => l.Updated)
                    .ThenBy(l => l.Id);
        }

        public static async Task<Lineup?> PreviousOrLast(this DbSet<Lineup> lineups, int id)
        {
            //todo: includes/joins in raw query?
            //performances should be max 24 records

            var previous = await lineups
                .FromSqlInterpolated($"SELECT * FROM lineups AS t WHERE t.Id = (SELECT subquery.Previous FROM ( SELECT t.*, LAG(t.Id) OVER (ORDER BY t.Doors, t.Name, t.Created, t.Updated, t.Id) AS Previous FROM lineups AS t) AS subquery WHERE subquery.Id = {id})")
                .IncludeAll()
                .FirstOrDefaultAsync();

            if (previous is not null)
            {
                return previous;
            }

            return await lineups
                .Sort()
                .IncludeAll()
                .LastOrDefaultAsync();
        }

        public static async Task<Lineup?> NextOrFirst(this DbSet<Lineup> lineups, int id)
        {
            //todo: includes/joins in raw query?
            //performances should be max 24 records

            var next = await lineups
                .FromSqlInterpolated($"SELECT * FROM lineups AS t WHERE t.Id = (SELECT subquery.Previous FROM ( SELECT t.*, LEAD(t.Id) OVER (ORDER BY t.Doors, t.Name, t.Created, t.Updated, t.Id) AS Previous FROM lineups AS t) AS subquery WHERE subquery.Id = {id})")
                .IncludeAll()
                .FirstOrDefaultAsync();

            if (next is not null)
            {
                return next;
            }

            return await lineups
                .Sort()
                .IncludeAll()
                .FirstOrDefaultAsync();
        }

        public static LineupResult ToLineupResult(this Lineup lineup)
        {
            return new LineupResult
            {
                Doors = lineup.Doors,
                Event = lineup.Event != null ? new EventResult { Id = lineup.Event.Id, Name = lineup.Event.Name } : null,
                ImageResult = lineup.Image == null ? null : new ImageResult { Alt = lineup.Image.Alt, ContentType = lineup.Image.ContentType, Created = lineup.Image.Created, Id = lineup.Image.Id, Updated = lineup.Image.Updated },
                Id = lineup.Id,
                Name = lineup.Name,
                IsSoldOut = lineup.IsSoldOut,
                LineupPerformanceResults = lineup.Performances.Select(p => new LineupPerformanceResult
                {
                    BandcampId = p.BandcampId,
                    BandcampLink = p.BandcampLink,
                    Description = p.Description,
                    Duration = p.Duration,
                    Id = p.Id,
                    Name = p.Name,
                    Spotify = p.Spotify,
                    Start = p.Start,
                    Type = p.Type,
                    YouTube = p.YouTube,
                    ImageResult = p.Image == null ? null : new ImageResult { Alt = p.Image.Alt, ContentType = p.Image.ContentType, Created = p.Image.Created, Id = p.Image.Id, Updated = p.Image.Updated },
                    ArtistResult = p.Artist == null
                    ? throw new NullReferenceException("The performance does not have an artist")
                    : new ArtistResult
                    {
                        Id = p.Artist.Id,
                        Name = p.Artist.Name,
                        Description = p.Artist.Description,
                        ImageResult = p.Artist.Image == null ? null : new ImageResult { Alt = p.Artist.Image.Alt, ContentType = p.Artist.Image.ContentType, Created = p.Artist.Image.Created, Id = p.Artist.Image.Id, Updated = p.Artist.Image.Updated }
                    }
                }).ToList()
            };
        }

        public static IQueryable<Lineup> IncludeAll(this IQueryable<Lineup> lineups)
        {
            return lineups
                .Include(l => l.Image)
                .Include(l => l.Performances.Take(24))
                .ThenInclude(p => p.Artist).ThenInclude(a => a != null ? a.Image : null);
        }
    }
}
