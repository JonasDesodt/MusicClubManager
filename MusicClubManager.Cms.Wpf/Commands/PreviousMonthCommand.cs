using MusicClubManager.Cms.Wpf.Extensions;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class PreviousMonthCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            App.Current.GetCalendarViewModel()?.SubtractMonth().Update();
        }
    }
}