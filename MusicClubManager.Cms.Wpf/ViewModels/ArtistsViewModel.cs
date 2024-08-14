using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Cms.Wpf.Interfaces;
using MusicClubManager.Cms.Wpf.Commands;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    internal class ArtistsViewModel : ViewModelBase, IUpdate<ArtistResult>, IFilter<ArtistFilter>
    {
        private readonly IArtistService _artistApiService;

        private PagedServiceResult<IList<ArtistResult>>? _pagedServiceResult;
        public PagedServiceResult<IList<ArtistResult>>? PagedServiceResult {
            get => _pagedServiceResult;
            set => SetProperty(ref _pagedServiceResult, value);
        }

        public ArtistsViewModel(IArtistService artistApiService)
        {
            _artistApiService = artistApiService;

            FilterCommand = new FilterCommand<ArtistFilter>(this);

            UpdateCommand = new UpdateCommand<ArtistResult>(this);

            Fetch();
        }

        private async void Fetch()
        {
            PagedServiceResult = await _artistApiService.GetAll(new PaginationRequest { Page = 1, PageSize = 24 }, new ArtistFilter { });
        }

        public async Task Fetch(ArtistFilter filter)
        {
            PagedServiceResult = await _artistApiService.GetAll(new PaginationRequest { Page = 1, PageSize = 24 }, filter);
        }

        public FilterCommand<ArtistFilter> FilterCommand { get; set; }

        public UpdateCommand<ArtistResult> UpdateCommand { get; set; }

        public async Task Update(ArtistResult result)
        {
            await _artistApiService.Update(result.Id, new ArtistRequest
            {
                Name = result.Name,
                Description = result.Description,
                ImageId = result.ImageResult?.Id
            });

            Fetch();
        }
    }
}
