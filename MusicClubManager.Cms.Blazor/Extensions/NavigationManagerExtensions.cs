using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MusicClubManager.Cms.Blazor.Models.Contexts;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Cms.Blazor.Extensions
{
    public static class NavigationManagerExtensions
    {
        public static PaginationRequest? GetPaginationRequest(this NavigationManager navigationManager)
        {
            var parsedQuery = QueryHelpers.ParseQuery(new Uri(navigationManager.Uri).Query);

            if (parsedQuery.TryGetValue("page", out StringValues pageValue) && int.TryParse(pageValue, out int page) && parsedQuery.TryGetValue("pageSize", out StringValues pageSizeValue) && int.TryParse(pageSizeValue, out int pageSize))
            {
                return new PaginationRequest { Page = (uint)page, PageSize = (uint)pageSize };
            }

            return null;
        }

        public static Pagination? GetPagination(this NavigationManager navigationManager)
        {
            var parsedQuery = QueryHelpers.ParseQuery(new Uri(navigationManager.Uri).Query);

            if (parsedQuery.TryGetValue("page", out StringValues pageValue) && int.TryParse(pageValue, out int page) && parsedQuery.TryGetValue("pageSize", out StringValues pageSizeValue) && int.TryParse(pageSizeValue, out int pageSize))
            {
                return new Pagination { Page = page, PageSize = pageSize };
            }

            return null;
        }

        public static string GetQuery(this NavigationManager navigationManager)
        {
            return new Uri(navigationManager.Uri).Query;
        }
    }
}
