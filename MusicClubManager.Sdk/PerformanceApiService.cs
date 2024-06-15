using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Sdk
{
    public class PerformanceApiService : IPerformanceService
    {
        public Task<ServiceResult<PerformanceResult>> Create(PerformanceRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<PerformanceResult>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<PerformanceResult>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedServiceResult<IList<PerformanceResult>>> GetAll(PaginationRequest paginationRequest, PerformanceFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<PerformanceResult>> Update(int id, PerformanceRequest request)
        {
            throw new NotImplementedException();
        }
    }
}