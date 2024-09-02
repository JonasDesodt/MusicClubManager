using MusicClubManager.Dto.Result;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class CalendarViewModel
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public IList<PerformanceResult> PerformanceResults { get; set; } = [];
    }
}
