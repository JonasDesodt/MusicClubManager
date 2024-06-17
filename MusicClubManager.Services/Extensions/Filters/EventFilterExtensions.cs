using MusicClubManager.Dto.Filters;
using MusicClubManager.Models;

namespace MusicClubManager.Services.Extensions.Filters
{
    public static class EventFilterExtensions
    {
        public static IQueryable<Event> AddFilter(this IQueryable<Event> events, EventFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                events = events.Where(e => e.Name.ToLower().Contains(filter.Name.Trim().ToLower()));
            }

            return events;
        }
    }
}
