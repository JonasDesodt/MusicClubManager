using MusicClubManager.Cms.Wpf.ViewModels;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class NavigateCommand(MainViewModel mainViewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if(parameter is string route)
            {
                mainViewModel.NavigateToAgenda(route);
            }       
        }
    }
}
