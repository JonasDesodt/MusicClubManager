using Microsoft.AspNetCore.Mvc;
using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Ui.Mvc.Models;

namespace MusicClubManager.Ui.Mvc.Controllers
{
    public class AgendaController(ILineupService lineupApiService) : Controller
    {
        [Route("Agenda")]
        public async Task<IActionResult> Index([FromQuery] Pagination? pagination = null)
        {
            pagination ??= new Pagination ();

            return View(await lineupApiService.GetAll(new PaginationRequest { Page = (uint)pagination.Page, PageSize = (uint)pagination.PageSize }, new LineupFilter()));
        }

        [Route("Agenda/{id:int}")]
        public async Task<IActionResult> Lineup(int id)
        {
            return View("Lineup", await lineupApiService.Get(id, new PaginationRequest {  Page = 1, PageSize = 4} ));
        }
    }
}
