using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Abstractions
{
    public interface IArtistService : IService<ArtistRequest, ArtistResult, ArtistFilter> { }
}