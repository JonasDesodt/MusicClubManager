using MusicClubManager.Blazor.Services;
using MusicClubManager.Dto.Filters;

namespace MusicClubManager.Blazor.Stores
{
    public class ArtistFilterStore(LocalStorageService localStorageService)
    {
        public async Task<ArtistFilter> Get()
        {
            return await localStorageService.GetItem<ArtistFilter>("ArtistFilter") ?? new ArtistFilter { };
        }

        public async Task Set(ArtistFilter artistFilter)
        {
            await localStorageService.SetItem("ArtistFilter", artistFilter);
        }

        public async Task Remove() 
        {
            await localStorageService.RemoveItem("ArtistFilter");
        }
    }
}