using MusicClubManager.Blazor.Services;
using MusicClubManager.Dto.Filters;

namespace MusicClubManager.Blazor.Stores
{
    public class LineupFilterStore(LocalStorageService localStorageService)
    {
        public async Task<LineupFilter> Get()
        {
            return await localStorageService.GetItem<LineupFilter>("LineupFilter") ?? new LineupFilter { };
        }

        public async Task Set(LineupFilter lineupFilter)
        {
            await localStorageService.SetItem("LineupFilter", lineupFilter);
        }

        public async Task Remove()
        {
            await localStorageService.RemoveItem("LineupFilter");
        }
    }
}
