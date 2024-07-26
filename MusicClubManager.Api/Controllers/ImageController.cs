using Microsoft.AspNetCore.Mvc;
using MusicClubManager.Core;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Models;
using MusicClubManager.Services;
using System.Diagnostics.CodeAnalysis;
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

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] ImageRequest properties)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded."); //return serviceresult?
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
                        Alt = properties.Alt,
                        Created = now,
                        Updated = now,
                        Content = memoryStream.ToArray(),
                        ContentType = file.ContentType
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

        [HttpPut("Properties/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, ImageRequest properties)
        {
            //if (model.BrowserFile is { Size: > 0 } file)

            var image = await dbContext.Images.FindAsync(id);
            if (image is null)
            {
                return BadRequest("The image could not be found."); //return serviceresult?
            }

            image.Alt = properties.Alt;
            image.Updated = DateTime.UtcNow;

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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] ImageRequest properties, IFormFile formFile)
        {
            if (formFile is not { Length: > 0 } file)
            {
                return BadRequest("No file uploaded."); //return serviceresult?
            }

            var image = await dbContext.Images.FindAsync(id);
            if (image is null)
            {
                return BadRequest("The image could not be found."); //return serviceresult?
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    image.Content = memoryStream.ToArray();

                    image.Alt = properties.Alt;
                    image.ContentType = file.ContentType;
                    image.Updated = DateTime.UtcNow;

                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    return BadRequest("The image is too large"); //return serviceresult?
                }
            }

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


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await imageDbService.Get(id));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest paginationRequest)
        {
            return Ok(await imageDbService.GetAll(paginationRequest));
        }
    }
}
