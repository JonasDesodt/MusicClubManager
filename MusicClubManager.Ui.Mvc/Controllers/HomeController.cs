using Microsoft.AspNetCore.Mvc;
using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Ui.Mvc.Models;
using MusicClubManager.Ui.Mvc.Models.ViewModels;
using System.Diagnostics;

namespace MusicClubManager.Ui.Mvc.Controllers
{
    public class HomeController(ILineupService lineupApiService, ILogger<HomeController> logger) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var confirmed = await lineupApiService.GetAll(new PaginationRequest { Page = 2, PageSize = 3 }, new LineupFilter());
            var soon = await lineupApiService.GetAll(new PaginationRequest { Page = 1, PageSize = 3 }, new LineupFilter());

            return View(new HomeViewModel { Confirmed = confirmed, Soon = soon });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
