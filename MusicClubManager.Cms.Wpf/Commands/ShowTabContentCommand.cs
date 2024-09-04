using MusicClubManager.Cms.Wpf.Extensions;
using MusicClubManager.Cms.Wpf.Interfaces;
using MusicClubManager.Cms.Wpf.Models;
using MusicClubManager.Cms.Wpf.ViewModels;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class ShowTabContentCommand(Tab tab) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            App.Current.GetMainViewModel()?.ShowCurrentTabContent(tab.ViewModel);
        }
    }
}
