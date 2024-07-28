using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Sdk.Extensions
{
   public static class PaginationRequestExtensions
    {
        public static string ToQueryString(this PaginationRequest paginationRequest)
        {
            return $"page={paginationRequest.Page}&pageSize={paginationRequest.PageSize}";
        }
    }
}