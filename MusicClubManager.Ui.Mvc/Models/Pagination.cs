namespace MusicClubManager.Ui.Mvc.Models
{
    public class Pagination
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalCount { get; set; }

        public int TotalPages => TotalCount / PageSize;
        public int StartPage => (Page - 3) <= 0 ? 1 : Page - 3;
        public int EndPage => StartPage + 3 > TotalPages ? TotalPages : StartPage + 3;
    }
}