using Microsoft.AspNetCore.Mvc;
using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Services;

namespace MusicClubManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class PerformanceController(IPerformanceService performanceDbService) : ControllerBase
    {
        [HttpGet("by-lineup-id/{id:int}")]
        public async Task<IActionResult> GetAll([FromRoute] int id, [FromQuery] PaginationRequest paginationRequest, [FromQuery] PerformanceFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await performanceDbService.GetAll(id, paginationRequest, filter));
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest paginationRequest, [FromQuery] PerformanceFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await performanceDbService.GetAll(paginationRequest, filter));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await performanceDbService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PerformanceRequest performanceRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await performanceDbService.Create(performanceRequest));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PerformanceRequest performanceRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await performanceDbService.Update(id, performanceRequest));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await performanceDbService.Delete(id));
        }
    }
}
