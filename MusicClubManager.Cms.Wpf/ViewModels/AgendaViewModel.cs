using MusicClubManager.Dto.Transfer;
using MusicClubManager.Dto.Result;
using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class AgendaViewModel : ViewModelBase
    {
        private readonly IPerformanceService _performanceApiService;

        private PagedServiceResult<IList<PerformanceResult>>? _pagedServiceResult;
        public PagedServiceResult<IList<PerformanceResult>>? PagedServiceResult
        {
            get => _pagedServiceResult;
            set => SetProperty(ref _pagedServiceResult, value);
        }


        private PaginationViewModel? _paginationViewModel;
        public PaginationViewModel? PaginationViewModel
        {
            get => _paginationViewModel;
            set => SetProperty(ref _paginationViewModel, value);
        }


        public AgendaViewModel(IPerformanceService performanceApiService)
        {
            _performanceApiService = performanceApiService;

            Fetch(new PaginationRequest { Page = 1, PageSize = 1 });            
        }

        private async void Fetch(PaginationRequest paginationRequest)
        {
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (sender, args) =>
            {
                if (IsReady is true) IsReady = false;
            };

            timer.Start();

            PagedServiceResult = await _performanceApiService.GetAll(paginationRequest, new PerformanceFilter { });

            PaginationViewModel = new PaginationViewModel((int)PagedServiceResult.Page, (int)PagedServiceResult.PageSize, (int)PagedServiceResult.TotalCount)
            {
                OnFetchRequest = Fetch,
 
            };

            timer.Stop();
            timer.Dispose();

            IsReady = true;
        }
    }
}