using MusicClubManager.Cms.Wpf.Interfaces;
using MusicClubManager.Dto.Result;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class PerformanceViewModel(PerformanceResult source, string artistName) : ISelectable
    {
        public object Source { get; } = source;

        public string ArtistName { get; set; } = artistName;
    }
}
