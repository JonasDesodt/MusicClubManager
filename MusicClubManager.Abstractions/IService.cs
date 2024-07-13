using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Abstractions
{
    public interface IService<TRequest, TResult, TFilter> 
    {
        Task<ServiceResult<TResult>> Create(TRequest request);

        Task<ServiceResult<TResult>> Get(int id);

        Task<PagedServiceResult<IList<TResult>>> GetAll(PaginationRequest paginationRequest, TFilter filter);

        Task<ServiceResult<TResult>> Delete(int id);

        Task<ServiceResult<TResult>> Update(int id, TRequest request);
    }
}