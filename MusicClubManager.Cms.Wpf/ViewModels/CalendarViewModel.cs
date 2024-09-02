using MusicClubManager.Abstractions;
using MusicClubManager.Cms.Wpf.Models;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private readonly IPerformanceService _performanceApiService;

        public int Year { get; set; } = 2024;

        public int Month { get; set; } = 8;

        public Day[] Cells { get; private set; }           

        public CalendarViewModel(IPerformanceService performanceApiService)
        {
            Cells = GetCells();

            _performanceApiService = performanceApiService;

            Fetch(new PaginationRequest { Page = 1, PageSize = 24 });
        }

        private Day[] GetCells()
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

            for(int i = start, day = 1; day <= daysInMonth; i++, day++)
            {
                cells[i] = new Day { Date = new DateOnly(Year, Month, day) };
            }

            return cells;
        }

        private async void Fetch(PaginationRequest paginationRequest)
        {
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (sender, args) =>
            {
                if (IsReady is true) IsReady = false;
            };

            timer.Start();

            var pagedServiceResult = await _performanceApiService.GetAll(paginationRequest, new PerformanceFilter { });

            var paginationViewModel = new PaginationViewModel((int)pagedServiceResult.Page, (int)pagedServiceResult.PageSize, (int)pagedServiceResult.TotalCount)
            {
                OnFetchRequest = Fetch,

            };

            timer.Stop();
            timer.Dispose();

            if(pagedServiceResult.Data is not null)
            {
                foreach (var cell in Cells)
                {
                    if(cell?.Date is DateOnly dateOnly)
                    {
                        foreach(var performanceResult in pagedServiceResult.Data.Where(x => x.Start != null && x.Start.Value.Day == dateOnly.Day)){
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