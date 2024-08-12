using Microsoft.EntityFrameworkCore;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Models;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

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
            var previous = await lineups.FromSqlInterpolated($"SELECT * FROM lineups AS t WHERE t.Id = (SELECT subquery.Previous FROM ( SELECT t.*, LAG(t.Id) OVER (ORDER BY t.Doors, t.Name, t.Created, t.Updated, t.Id) AS Previous FROM lineups AS t) AS subquery WHERE subquery.Id = {id})").Include(l => l.Image).FirstOrDefaultAsync();

            if (previous is not null)
            {
                return previous;
            }

            return await lineups.Sort().Include(l => l.Event).Include(l => l.Image).LastOrDefaultAsync();
        }

        public static async Task<Lineup?> NextOrFirst(this DbSet<Lineup> lineups, int id)
        {
            //todo: includes/joins in raw query?
            var next = await lineups.FromSqlInterpolated($"SELECT * FROM lineups AS t WHERE t.Id = (SELECT subquery.Previous FROM ( SELECT t.*, LEAD(t.Id) OVER (ORDER BY t.Doors, t.Name, t.Created, t.Updated, t.Id) AS Previous FROM lineups AS t) AS subquery WHERE subquery.Id = {id})").Include(l => l.Image).FirstOrDefaultAsync();

            if (next is not null)
            {
                return next;
            }

            return await lineups.Sort().Include(l => l.Event).Include(l => l.Image).FirstOrDefaultAsync();
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
                PagedLineupPerformanceResult = new PagedResult<IList<LineupPerformanceResult>>
                {
                    Page = 1, //paginationRequest.Page,
                    PageSize = 24, //paginationRequest.PageSize,
                    TotalCount = 24//(uint)performancesTotalCount
                }
            };
        }
    }
}
