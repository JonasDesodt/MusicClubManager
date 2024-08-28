using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class PaginationViewModel: ViewModelBase
    {
        private int _page;
        public int Page
        {
            get => _page;
            set
            {
                if (value != _page)
                {
                    _page = value;

                    OnFetchRequest(new PaginationRequest { Page = (uint)Page, PageSize = (uint)PageSize });
                }

                //SetProperty(ref _page, value);
            }
        }

        private int _pageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value;
            //set => SetProperty(ref _pageSize, value);
        }

        private int _totalCount;
        public int TotalCount
        {
            get => _totalCount;
            set => _totalCount = value;
            //set => SetProperty(ref _totalCount, value);
        }

        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set => _totalPages = value;
        }


        public PaginationViewModel(int page, int pageSize, int totalCount)
        {
            _page = page;
            _pageSize = pageSize;
            _totalCount = totalCount;

            _totalPages = (int)Math.Ceiling(TotalCount / (decimal)PageSize);
        }

        public required Action<PaginationRequest> OnFetchRequest { get; set; }
    }
}
