using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Services
{
    public class EventDbService : IEventService
    {
        public Task<ServiceResult<EventResult>> Create(EventRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<EventResult>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<EventResult>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedServiceResult<IList<EventResult>>> GetAll(PaginationRequest paginationRequest, EventFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<EventResult>> Update(int id, EventRequest request)
        {
            throw new NotImplementedException();
        }
    }
}