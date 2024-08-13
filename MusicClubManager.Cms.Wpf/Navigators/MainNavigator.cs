using MusicClubManager.Abstractions;
using MusicClubManager.Cms.Wpf.ViewModels;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Cms.Wpf.Navigators
{
    public class MainNavigator
    {
        private readonly IPerformanceService _performanceApiService;

        private readonly object _viewModel;
        public MainNavigator(object viewModel, IPerformanceService performanceApiService)
        {
            _performanceApiService = performanceApiService;
            _viewModel = viewModel;

            FetchPerformances();
        }

        private async void FetchPerformances()
        {
            //((AgendaViewModel)_viewModel).PagedServiceResult = await _performanceApiService.GetAll(new PaginationRequest { Page = 1, PageSize = 24 }, new PerformanceFilter { });
        }
    }
}
