using Microsoft.AspNetCore.Mvc;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Services;
using Microsoft.AspNetCore.Authorization;
using MusicClubManager.Abstractions;

namespace MusicClubManager.Api.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("[controller]")]
    public class IdentityController(IIdentityService identityDbService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await identityDbService.Register(registerRequest));
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<IActionResult> GetToken([FromBody]TokenRequest tokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await identityDbService.GetToken(tokenRequest));
        }
    }
}