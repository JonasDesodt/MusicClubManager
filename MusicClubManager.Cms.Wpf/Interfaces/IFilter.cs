namespace MusicClubManager.Cms.Wpf.Interfaces
{
    public interface IFilter<TFilter>
    {
        Task Fetch(TFilter filter);
    }
}
