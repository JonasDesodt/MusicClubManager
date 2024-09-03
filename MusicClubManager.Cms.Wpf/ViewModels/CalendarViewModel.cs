using MusicClubManager.Abstractions;
using MusicClubManager.Cms.Wpf.Commands;
using MusicClubManager.Cms.Wpf.Extensions;
using MusicClubManager.Cms.Wpf.Models;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private readonly IPerformanceService _performanceApiService;

        private int _year = DateTime.UtcNow.Year;
        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        private int _month = DateTime.UtcNow.Month;
        public int Month
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }

        private Day[] _cells;
        public Day[] Cells
        {
            get => _cells;
            set => SetProperty(ref _cells, value);
        }

        public PreviousYearCommand PreviousYearCommand { get; set; }
        public NextYearCommand NextYearCommand { get; set; }

        public PreviousMonthCommand PreviousMonthCommand { get; set; }
        public NextMonthCommand NextMonthCommand { get; set; }


        private PerformanceResult? _selectedPerformanceResult;
        public PerformanceResult? SelectedPerformanceResult
        {
            get => _selectedPerformanceResult;
            set
            {
                _selectedPerformanceResult = value;

                // TODO, TEMP HACK, USE A COMMAND OR A BEHAVIOR
                App.Current.GetMainViewModel()?.AddTab(value);
            }
        }


        public CalendarViewModel(IPerformanceService performanceApiService)
        {
            PreviousYearCommand = new PreviousYearCommand();
            NextYearCommand = new NextYearCommand();

            PreviousMonthCommand = new PreviousMonthCommand();
            NextMonthCommand = new NextMonthCommand();

            _cells = GetCells();

            _performanceApiService = performanceApiService;

            Fetch(new PaginationRequest { Page = 1, PageSize = 24 }, new PerformanceFilter { Year = Year, Month = Month });
        }

        public Day[] GetCells()
        {
            var cells = new Day[42];

            var start = new DateTime(Year, Month, 1).DayOfWeek switch
            {
                DayOfWeek.Monday => 0,
                DayOfWeek.Tuesday => 1,
                DayOfWeek.Wednesday => 2,
                DayOfWeek.Thursday => 3,
                DayOfWeek.Friday => 4,
                DayOfWeek.Saturday => 5,
                _ => 6
            };

            var daysInMonth = DateTime.DaysInMonth(Year, Month);

            for (int i = start, day = 1; day <= daysInMonth; i++, day++)
            {
                cells[i] = new Day { Date = new DateOnly(Year, Month, day) };
            }

            return cells;
        }

        public async void Fetch(PaginationRequest paginationRequest, PerformanceFilter performanceFilter)
        {

            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (sender, args) =>
            {
                if (IsReady is true) IsReady = false;
            };

            timer.Start();

            var pagedServiceResult = await _performanceApiService.GetAll(paginationRequest, performanceFilter);

            var paginationViewModel = new PaginationViewModel((int)pagedServiceResult.Page, (int)pagedServiceResult.PageSize, (int)pagedServiceResult.TotalCount)
            {
                OnFetchRequest = Fetch,

            };

            timer.Stop();
            timer.Dispose();

            if (pagedServiceResult.Data is not null)
            {
                foreach (var cell in Cells)
                {
                    if (cell?.Date is DateOnly dateOnly)
                    {
                        foreach (var performanceResult in pagedServiceResult.Data.Where(x => x.Start != null && x.Start.Value.Day == dateOnly.Day))
                        {
                            cell.PerformanceResults.Add(performanceResult);
                        }

                        //cell.PerformanceResults = [.. cell.PerformanceResults, .. pagedServiceResult.Data.Where(p => p.Start != null && p.Start.Value.Day == dateOnly.Day)];
                    }
                }
            }

            IsReady = true;
        }
    }
}