using MusicClubManager.Cms.Wpf.Commands;
using MusicClubManager.Dto.Result;
using System.Collections.ObjectModel;

namespace MusicClubManager.Cms.Wpf.Models
{
    public class Day
    {
        public DateOnly? Date { get; set; }

        public ObservableCollection<PerformanceResult> PerformanceResults { get; set; } = [];
    }
}
