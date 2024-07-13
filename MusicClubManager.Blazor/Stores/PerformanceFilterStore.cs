using MusicClubManager.Blazor.Services;
using MusicClubManager.Dto.Filters;

namespace MusicClubManager.Blazor.Stores
{
    public class PerformanceFilterStore(LocalStorageService localStorageService)
    {
        public async Task<PerformanceFilter> Get()
        {
            return await localStorageService.GetItem<PerformanceFilter>("PerformanceFilter") ?? new PerformanceFilter { };
        }

        public async Task Set(PerformanceFilter PerformanceFilter)
        {
            await localStorageService.SetItem("PerformanceFilter", PerformanceFilter);
        }

        public async Task Remove()
        {
            await localStorageService.RemoveItem("PerformanceFilter");
        }
    }
}
