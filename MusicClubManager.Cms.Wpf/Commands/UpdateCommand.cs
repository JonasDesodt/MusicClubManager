using MusicClubManager.Cms.Wpf.Interfaces;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class UpdateCommand<TResult>(IUpdate<TResult> viewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            if(parameter is TResult result)
            {
                await viewModel.Update(result);
            }
        }
    }
}
