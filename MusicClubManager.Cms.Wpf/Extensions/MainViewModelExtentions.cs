using MusicClubManager.Cms.Wpf.Interfaces;
using MusicClubManager.Cms.Wpf.ViewModels;

namespace MusicClubManager.Cms.Wpf.Extensions
{
    public static class MainViewModelExtentions
    {
        public static MainViewModel HideCurrentTabContent(this MainViewModel mainViewModel)
        {
            mainViewModel.CurrentTabContent = null;

            return mainViewModel;
        }

        public static MainViewModel ShowCurrentTabContent(this MainViewModel mainViewModel, ITabContent tabContent)
        {
            mainViewModel.CurrentTabContent = tabContent;

            return mainViewModel;
        }
    }
}
