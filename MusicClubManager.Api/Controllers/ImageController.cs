using Microsoft.AspNetCore.Mvc;
using MusicClubManager.Core;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Models;
using MusicClubManager.Services;
using System.Text.Json;


namespace MusicClubManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController(MusicClubManagerDbContext dbContext, ImageDbService imageDbService) : ControllerBase
    {
        [HttpGet("Download/{id:int}")]
        public async Task<IActionResult> Download(int id)
        {
            var image = await dbContext.Images.FindAsync(id);
            if (image is null)
            {
                return NotFound(); // return serviceResult
            }

            // Create a MemoryStream from the byte array
            var memoryStream = new MemoryStream(image.Content);

            // Return the file as a PhysicalFileResult
            return new FileStreamResult(memoryStream, image.ContentType)
            {
                FileDownloadName = image.Alt
            };
        }



        //[HttpGet("{name}")]
        //public IActionResult Get(string name)
        //{
        //    var filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"), name);
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return NotFound();
        //    }

        //    var mimeType = "image/png";
        //    return PhysicalFile(filePath, mimeType);
        //}

        [HttpPost]
        [Route("Upload")]
        //public async Task<IActionResult> Upload(IFormFile file,ImageRequest request)
        public async Task<IActionResult> Upload(IFormFile file, string json)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded."); //return serviceresult?
            }

            if (string.IsNullOrEmpty(json))
            {
                return BadRequest("No request recieved");
            }

            var request = JsonSerializer.Deserialize<ImageRequest>(json);
            if (request is null)
            {
                return BadRequest("Unable to deserialize the json string");
            }

            var now = DateTime.UtcNow;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var image = new Image
                    {
                        Alt = request.Alt,
                        Created = now,
                        Updated = now,
                        Content = memoryStream.ToArray(),
                        ContentType = request.ContentType
                    };

                    dbContext.Images.Add(image);

                    await dbContext.SaveChangesAsync();

                    return Ok(new ServiceResult<ImageResult>
                    {
                        Data = new ImageResult
                        {
                            Alt = image.Alt,
                            Created = image.Created,
                            Updated = image.Updated,
                            Id = image.Id,
                            ContentType = image.ContentType
                        }
                    });
                }
                else
                {
                    return BadRequest("The file is too large.");  //return serviceresult?
                }
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> Upload(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("No file uploaded.");
        //    }

        //    var filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/" + file.FileName));

        //    if (System.IO.File.Exists(filePath))
        //    {
        //        return BadRequest("Duplicate file name");
        //    }


        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }


        //    return Ok(new { Url = Url.Action(nameof(Get), new { fileName = file.FileName }) });
        //}

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await imageDbService.Get(id));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await imageDbService.GetAll());
        }
    }
}
