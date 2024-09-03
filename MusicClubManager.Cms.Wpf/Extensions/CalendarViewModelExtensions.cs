using MusicClubManager.Cms.Wpf.ViewModels;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Cms.Wpf.Extensions
{
    public static class CalendarViewModelExtensions
    {
        private const int MinYear = 1900;
        private static readonly int MaxYear = DateTime.UtcNow.AddYears(5).Year;

        public static CalendarViewModel SubtractYear(this CalendarViewModel calendarViewModel)
        {
            calendarViewModel.Year = calendarViewModel.Year - 1 > MinYear ? calendarViewModel.Year - 1 : MaxYear;

            return calendarViewModel;
        }

        public static CalendarViewModel AddYear(this CalendarViewModel calendarViewModel)
        {
            calendarViewModel.Year = calendarViewModel.Year + 1 < MaxYear ? calendarViewModel.Year + 1 : MinYear;

            return calendarViewModel;
        }

        public static CalendarViewModel SubtractMonth(this CalendarViewModel calendarViewModel)
        {
            if (calendarViewModel.Month - 1 >= 1)
            {
                calendarViewModel.Month--;
            }
            else
            {
                calendarViewModel.Month = 12;

                calendarViewModel.SubtractYear();
            }

            return calendarViewModel;
        }

        public static CalendarViewModel AddMonth(this CalendarViewModel calendarViewModel)
        {
            if (calendarViewModel.Month < 12)
            {
                calendarViewModel.Month++;
            }
            else
            {
                calendarViewModel.Month = 1;

                calendarViewModel.AddYear();
            }

            return calendarViewModel;
        }

        public static CalendarViewModel Update(this CalendarViewModel calendarViewModel)
        {
            calendarViewModel.UpdateCells().UpdateData();

            return calendarViewModel;
        }


        public static CalendarViewModel UpdateCells(this CalendarViewModel calendarViewModel)
        {
            calendarViewModel.Cells = calendarViewModel.GetCells();

            return calendarViewModel;
        }

        public static CalendarViewModel UpdateData(this CalendarViewModel calendarViewModel)
        {
            calendarViewModel.Fetch(new PaginationRequest { Page = 1, PageSize = 24 }, new PerformanceFilter { Year = calendarViewModel.Year, Month = calendarViewModel.Month });

            return calendarViewModel;
        }
    }
}
