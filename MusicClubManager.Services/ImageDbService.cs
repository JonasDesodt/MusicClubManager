using MusicClubManager.Core;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using Microsoft.EntityFrameworkCore;

namespace MusicClubManager.Services
{
    public class ImageDbService(MusicClubManagerDbContext dbContext)
    {
        //public async Task<ServiceResult<ImageResult>> Create(byte[] file, ImageRequest request)
        //{
        //    return null;
        //}

        public async Task<ServiceResult<ImageResult>> Get(int id)
        {
            var image = await dbContext.Images.FindAsync(id);

            if(image is null)
            {
                return new ServiceResult<ImageResult>()
                {
                    Messages =
                    [
                        new ServiceMessage
                        {
                            Message = $"The image with id {id} could not be found."
                        }
                    ]
                };
            }

            return new ServiceResult<ImageResult>
            {
                Data = new ImageResult
                {
                    Alt = image.Alt,
                    ContentType = image.ContentType,
                    Created = image.Created,
                    Updated = image.Updated,
                    Id = image.Id
                }
            };
        }

        public async Task<ServiceResult<IList<ImageResult>>> GetAll()
        {
              return new ServiceResult<IList<ImageResult>>
            {
                Data = await dbContext.Images.Select(i => new ImageResult
                {
                    Alt = i.Alt,
                    ContentType = i.ContentType,
                    Created = i.Created,
                    Updated = i.Updated,
                    Id = i.Id
                }).ToListAsync()
            };
        }
    }
}
