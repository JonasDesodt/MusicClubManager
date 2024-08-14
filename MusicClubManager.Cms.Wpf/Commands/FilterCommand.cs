using MusicClubManager.Cms.Wpf.Interfaces;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class FilterCommand<TFilter>(IFilter<TFilter> viewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            if(parameter is TFilter filter)
            {
                await viewModel.Fetch(filter);
            }
        }
    }
}
