using Microsoft.AspNetCore.Mvc;

namespace MusicClubManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"), name);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var mimeType = "image/png";
            return PhysicalFile(filePath, mimeType);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/" + file.FileName));

            if (System.IO.File.Exists(filePath))
            {
                return BadRequest("Duplicate name");
            }


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { Url = Url.Action(nameof(Get), new { fileName = file.FileName }) });
        }
    }
}
