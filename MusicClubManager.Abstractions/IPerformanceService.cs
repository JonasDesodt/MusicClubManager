using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Abstractions
{
    public interface IPerformanceService : IService<PerformanceRequest, PerformanceResult, PerformanceFilter> 
    {
        Task<PagedServiceResult<IList<LineupPerformanceResult>>> GetAll(int id, PaginationRequest paginationRequest, PerformanceFilter filter);
    }
}