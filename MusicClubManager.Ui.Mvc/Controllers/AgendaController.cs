using Microsoft.AspNetCore.Mvc;
using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Ui.Mvc.Controllers
{
    public class AgendaController(ILineupService lineupApiService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await lineupApiService.GetAll(new PaginationRequest { Page = 1, PageSize = 4 }, new LineupFilter()));
        }

        [Route("Agenda/{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            return View("Lineup", await lineupApiService.Get(id, new PaginationRequest {  Page = 1, PageSize = 4} ));
        }
    }
}
