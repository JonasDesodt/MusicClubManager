using MusicClubManager.Cms.Wpf.Commands;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class PaginationViewModel
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
            }
        }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }


        public PreviousPageCommand PreviousPageCommand { get; set; }
        public NextPageCommand NextPageCommand { get; set; }


        public PaginationViewModel(int page, int pageSize, int totalCount)
        {
            _page = page;

            PageSize = pageSize;
            TotalCount = totalCount;

            TotalPages = (int)Math.Ceiling(TotalCount / (decimal)PageSize);

            PreviousPageCommand = new PreviousPageCommand(this); 
            NextPageCommand = new NextPageCommand(this);
        }

        public required Action<PaginationRequest> OnFetchRequest { get; set; }
    }
}
