using MusicClubManager.Models;
using System.Linq;

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
    }
}
