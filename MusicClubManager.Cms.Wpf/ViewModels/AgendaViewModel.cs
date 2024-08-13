using MusicClubManager.Dto.Transfer;
using MusicClubManager.Dto.Result;
using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class AgendaViewModel : ViewModelBase
    {
        private readonly IPerformanceService _performanceApiService;

        public AgendaViewModel(IPerformanceService performanceApiService)
        {
            _performanceApiService = performanceApiService;

            Fetch();
        }

        private IList<PerformanceResult>? _data;
        public IList<PerformanceResult>? Data
        {
            get => _data;

            set => SetProperty(ref _data, value);
        }


        //public IPerformanceService? PerformanceApiService
        //{
        //    get => _performanceApiService;
        //    set
        //    {
        //        _performanceApiService = value;
        //        Fetch();
        //    }
        //}
        private async void Fetch()
        {
            //if (_performanceApiService is null)
            //{
            //    return;
            //}

            Data = (await _performanceApiService.GetAll(new PaginationRequest { Page = 1, PageSize = 24 }, new PerformanceFilter { })).Data;
        }
    }
}
