using MusicClubManager.Cms.Wpf.ViewModels;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class NextPageCommand(PaginationViewModel paginationViewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (paginationViewModel.Page >= paginationViewModel.TotalPages) return false;

            return true;
        }

        public void Execute(object? parameter)
        {
            paginationViewModel.Page++;
        }
    }
}