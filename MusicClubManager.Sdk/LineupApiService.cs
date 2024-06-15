using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Sdk
{
    public class LineupApiService : ILineupService
    {
        public Task<ServiceResult<LineupResult>> Create(LineupRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<LineupResult>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<LineupResult>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedServiceResult<IList<LineupResult>>> GetAll(PaginationRequest paginationRequest, LineupFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<LineupResult>> Update(int id, LineupRequest request)
        {
            throw new NotImplementedException();
        }
    }
}