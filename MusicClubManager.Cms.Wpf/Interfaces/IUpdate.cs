namespace MusicClubManager.Cms.Wpf.Interfaces
{
    public interface IUpdate<TRequest> //todo: use IService from abstractions?
    {
        TRequest? GetRequest();
        
        Task Update(int id, TRequest request);

        
    }
}
