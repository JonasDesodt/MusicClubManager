using Microsoft.AspNetCore.Mvc;
using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Transfer;
using Microsoft.AspNetCore.Authorization;

namespace MusicClubManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class ArtistController(IArtistService artistDbService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest paginationRequest, [FromQuery] ArtistFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await artistDbService.GetAll(paginationRequest, filter));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await artistDbService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArtistRequest artistRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await artistDbService.Create(artistRequest));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id,  [FromBody] ArtistRequest artistRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await artistDbService.Update(id, artistRequest));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await artistDbService.Delete(id));
        }
    }
}