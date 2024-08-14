using MusicClubManager.Cms.Wpf.Interfaces;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class SelectCommand<TModel>(ISelect<TModel> viewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if(parameter is TModel item)
            {
                viewModel.Select(item);
            }
        }
    }
}
