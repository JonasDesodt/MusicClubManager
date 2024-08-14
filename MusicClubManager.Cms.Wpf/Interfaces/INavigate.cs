namespace MusicClubManager.Cms.Wpf.Interfaces
{
    public interface INavigate
    {
        object? CurrentViewModel { get; set; }

        void Navigate(string route);
    }
}
