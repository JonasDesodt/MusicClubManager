using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Cms.Wpf.Interfaces;
using MusicClubManager.Cms.Wpf.Commands;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    internal class ArtistsViewModel : ViewModelBase, ISelect<ArtistResult>
    {
        private readonly IArtistService _artistApiService;

        private PagedServiceResult<IList<ArtistResult>>? _pagedServiceResult;
        public PagedServiceResult<IList<ArtistResult>>? PagedServiceResult {
            get => _pagedServiceResult;
            set => SetProperty(ref _pagedServiceResult, value);
        }

        private ArtistResult? _selectedItem;
        public ArtistResult? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public SelectCommand<ArtistResult> SelectCommand { get; set; }

        public ArtistsViewModel(IArtistService artistApiService)
        {
            _artistApiService = artistApiService;

            SelectCommand = new SelectCommand<ArtistResult>(this);

            Fetch();
        }

        private async void Fetch()
        {
            PagedServiceResult = await _artistApiService.GetAll(new PaginationRequest { Page = 1, PageSize = 24 }, new ArtistFilter { });
        }

        public void Select(ArtistResult item)
        {
            _selectedItem = item;
        }
    }
}
