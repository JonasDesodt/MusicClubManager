using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class HideTabCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
