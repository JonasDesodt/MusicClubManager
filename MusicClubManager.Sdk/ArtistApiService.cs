using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Sdk
{
    public class ArtistApiService : IArtistService
    {
        public Task<ServiceResult<ArtistResult>> Create(ArtistRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<ArtistResult>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<ArtistResult>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedServiceResult<IList<ArtistResult>>> GetAll(PaginationRequest paginationRequest, ArtistFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<ArtistResult>> Update(int id, ArtistRequest request)
        {
            throw new NotImplementedException();
        }
    }
}