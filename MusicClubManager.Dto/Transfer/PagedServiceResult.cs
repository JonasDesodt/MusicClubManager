namespace MusicClubManager.Dto.Transfer
{
    public class PagedServiceResult<T> : ServiceResult<T>
    {
        public required uint Page { get; set; }

        public required uint PageSize { get; set; }

        public required uint TotalCount { get; set; }
    }
}