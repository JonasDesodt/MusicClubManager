using MusicClubManager.Cms.Blazor.Models.Contexts;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Cms.Blazor.Extensions
{
    public static class PaginationExtensions
    {
        public static PaginationRequest ToPaginationRequest(this Pagination pagination)
        {
            return new PaginationRequest
            {
                Page = (uint)pagination.Page,
                PageSize = (uint)pagination.PageSize,
            };
        }
    }
}
