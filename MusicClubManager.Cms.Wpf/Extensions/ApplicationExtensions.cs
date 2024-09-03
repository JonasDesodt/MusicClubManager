using MusicClubManager.Cms.Wpf.ViewModels;
using System.Windows;

namespace MusicClubManager.Cms.Wpf.Extensions
{
    public static class ApplicationExtensions
    {
        public static CalendarViewModel? GetCalendarViewModel(this Application current)
        {
            if (current.Windows.OfType<MainWindow>().FirstOrDefault()?.DataContext is not MainViewModel vm || vm.CurrentViewModel is not CalendarViewModel viewModel)
            {
                return null;
            }

            return viewModel;
        }
    }
}