namespace MusicClubManager.Cms.Wpf.Interfaces
{
    public interface IUpdate<TResult>
    {       
        Task Update(TResult result);       
    }
}
