using MusicClubManager.Cms.Wpf.ViewModels;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class PreviousMonthCommand(CalendarViewModel calendarViewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            calendarViewModel.Month = calendarViewModel.Month - 1 >= 1 ? calendarViewModel.Month - 1 : 12;

            calendarViewModel.Fetch(new PaginationRequest {  Page  = 1, PageSize = 24 }, new PerformanceFilter { Year = calendarViewModel.Year, Month = calendarViewModel.Month });
        }
    }
}
