using MusicClubManager.Dto.Transfer;
using MusicClubManager.Dto.Result;
using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class AgendaViewModel : ViewModelBase
    {
        private IPerformanceService? _performanceApiService;
        public IPerformanceService? PerformanceApiService
        {
            get => _performanceApiService;
            set
            {
                _performanceApiService = value;
                Fetch();
            }
        }


        private IList<PerformanceResult>? _data;
        public IList<PerformanceResult>? Data
        {
            get => _data;

            set => SetProperty(ref _data, value);
        }

        private async void Fetch()
        {
            if (_performanceApiService is null)
            {
                return;
            }

            Data = (await _performanceApiService.GetAll(new PaginationRequest { Page = 1, PageSize = 24 }, new PerformanceFilter { })).Data;
        }
    }
}
