using MusicClubManager.Cms.Wpf.Interfaces;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class UpdateCommand<TRequest>(IUpdate<TRequest> viewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if(parameter is int id && viewModel.GetRequest() is TRequest request)
            {
                viewModel.Update(id, request);
            }
        }
    }
}
