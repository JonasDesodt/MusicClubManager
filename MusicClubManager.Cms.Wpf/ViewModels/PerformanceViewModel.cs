using MusicClubManager.Cms.Wpf.Interfaces;
using MusicClubManager.Dto.Result;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class PerformanceViewModel(int id, PerformanceResult source, string artistName) : ITabContent
    {
        public int Id { get; } = id;

        public object Source { get; } = source;

        public string ArtistName { get; set; } = artistName;
    }
}
