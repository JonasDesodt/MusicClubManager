
using MusicClubManager.Cms.Wpf.ViewModels;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class PreviousYearCommand(CalendarViewModel calendarViewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            calendarViewModel.Year = calendarViewModel.Year - 1 > 1990 ? calendarViewModel.Year - 1 : calendarViewModel.Year;

            calendarViewModel.Fetch(new PaginationRequest { Page = 1, PageSize = 24 }, new PerformanceFilter { Year = calendarViewModel.Year, Month = calendarViewModel.Month });
        }
    }
}
