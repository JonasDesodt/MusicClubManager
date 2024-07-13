namespace MusicClubManager.Dto.Result
{
    public class PagedResult<T>
    {
        public T? Data { get; set; }

        public required uint Page { get; set; }

        public required uint PageSize { get; set; }

        public required uint TotalCount { get; set; }
    }
}