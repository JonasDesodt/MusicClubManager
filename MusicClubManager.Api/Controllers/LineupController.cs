﻿using Microsoft.AspNetCore.Mvc;
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
    public class LineupController(ILineupService lineupDbService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest paginationRequest, [FromQuery] LineupFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await lineupDbService.GetAll(paginationRequest, filter));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] PaginationRequest? paginationRequest)
        {
            if(paginationRequest is null)
            {
                return Ok(await lineupDbService.Get(id));
            }

            return Ok(await lineupDbService.Get(id, paginationRequest));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LineupRequest lineupRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await lineupDbService.Create(lineupRequest));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] LineupRequest lineupRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await lineupDbService.Update(id, lineupRequest));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await lineupDbService.Delete(id));
        }
    }
}
