using MusicClubManager.Cms.Wpf.Extensions;
using MusicClubManager.Cms.Wpf.Models;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class CloseTabCommand(Tab tab) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            App.Current.GetMainViewModel()?.Tabs.Remove(tab);
        }
    }
}
