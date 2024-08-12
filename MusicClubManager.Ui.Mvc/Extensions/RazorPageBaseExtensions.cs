using Microsoft.AspNetCore.Mvc.Razor;

namespace MusicClubManager.Ui.Mvc.Extensions
{
    public static class RazorPageBaseExtensions
    {
        public static string? GetQueryParam(this RazorPageBase page, string key)
        {
            return page.ViewContext.HttpContext.Request.Query[key];
        }
    }
}
