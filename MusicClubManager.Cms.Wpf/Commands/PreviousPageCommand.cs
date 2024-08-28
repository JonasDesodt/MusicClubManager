using MusicClubManager.Cms.Wpf.ViewModels;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class PreviousPageCommand(PaginationViewModel paginationViewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (paginationViewModel.Page <= 1) return false;

            return true;
        }

        public void Execute(object? parameter)
        {
            paginationViewModel.Page--;
        }
    }
}