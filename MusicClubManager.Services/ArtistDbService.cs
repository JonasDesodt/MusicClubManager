using Microsoft.EntityFrameworkCore;
using MusicClubManager.Abstractions;
using MusicClubManager.Services.Extensions.Filters;
using MusicClubManager.Core;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Models;

namespace MusicClubManager.Services
{
    public class ArtistDbService(MusicClubManagerDbContext dbContext) : IArtistService
    {
        public async Task<ServiceResult<ArtistResult>> Create(ArtistRequest request)
        {
            var artist = new Artist
            {
                Name = request.Name,
                Description = request.Description,
                ImageId = request.ImageId
            };

            await dbContext.Artists.AddAsync(artist);

            await dbContext.SaveChangesAsync();


            return new ServiceResult<ArtistResult>
            {
                Data = new ArtistResult
                {
                    Id = artist.Id,
                    Name = artist.Name,
                    Description = artist.Description,
                    ImageResult = await dbContext.Images.Select(i => new ImageResult
                    {
                        Alt = i.Alt,
                        ContentType = i.ContentType,
                        Created = i.Created,
                        Updated = i.Updated,
                        Id = i.Id

                    }).FirstOrDefaultAsync(i => i.Id == artist.ImageId)
                }
            };
        }

        public async Task<ServiceResult<ArtistResult>> Delete(int id)
        {
            var artist = await dbContext.Artists.FindAsync(id);

            if (artist is null)
            {
                return new ServiceResult<ArtistResult>
                {
                    Messages =
                    [
                        new () { Message = "The artist has not been deleted. The artist could not be found." }
                    ]
                };
            }

            if (await dbContext.Performances.AnyAsync(p => p.LineupId == id))
            {
                return new ServiceResult<ArtistResult>
                {
                    Messages =
                    [
                        new () { Message = "The artist has not been deleted. There are performances associated with this artist." }
                    ]
                };
            }

            dbContext.Artists.Remove(artist);

            await dbContext.SaveChangesAsync();

            return new ServiceResult<ArtistResult> { };
        }

        public async Task<ServiceResult<ArtistResult>> Get(int id)
        {
            var artist = await dbContext.Artists.Include(a => a.Image).FirstOrDefaultAsync(a => a.Id == id);

            if (artist is null)
            {
                return new ServiceResult<ArtistResult>
                {
                    Messages =
                    [
                        new () { Message = "The artist could not be found." }
                    ]
                };
            }

            return new ServiceResult<ArtistResult>
            {
                Data = new ArtistResult
                {
                    Id = artist.Id,
                    Name = artist.Name,
                    Description = artist.Description,
                    ImageResult = artist.Image != null
                    ? new ImageResult
                    {
                        Alt = artist.Image.Alt,
                        ContentType = artist.Image.ContentType,
                        Created = artist.Image.Created,
                        Updated = artist.Image.Updated,
                        Id = artist.Image.Id
                    }
                    : null
                }
            };
        }

        public async Task<PagedServiceResult<IList<ArtistResult>>> GetAll(PaginationRequest paginationRequest, ArtistFilter filter)
        {
            var totalCount = await dbContext.Artists
                .Include(a => a.Image)
                .AddFilter(filter)
                .CountAsync();

            var skip = (paginationRequest.Page - 1) * paginationRequest.PageSize;

            var artists = await dbContext.Artists
                .Include(a => a.Image)
                .AddFilter(filter)
                .Skip((int)skip)
                .Take((int)paginationRequest.PageSize)

                .Select(a => new ArtistResult
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    ImageResult = a.Image != null
                                  ? new ImageResult
                                  {
                                      Alt = a.Image.Alt,
                                      ContentType = a.Image.ContentType,
                                      Created = a.Image.Created,
                                      Updated = a.Image.Updated,
                                      Id = a.Image.Id
                                  }
                                  : null
                })
                .ToListAsync();

            return new PagedServiceResult<IList<ArtistResult>>
            {
                Data = artists,
                TotalCount = (uint)totalCount,
                PageSize = paginationRequest.PageSize,
                Page = paginationRequest.Page,
            };
        }

        public async Task<ServiceResult<ArtistResult>> Update(int id, ArtistRequest request)
        {
            var artist = await dbContext.Artists.Include(a => a.Image).FirstOrDefaultAsync( a => a.Id == id);

            if (artist is null)
            {
                return new ServiceResult<ArtistResult>
                {
                    Messages =
                    [
                        new () { Message = "The artist could not be found." }
                    ]
                };
            }

            artist.Name = request.Name;
            artist.Description = request.Description;
            artist.ImageId = request.ImageId;

            dbContext.Artists.Update(artist);

            await dbContext.SaveChangesAsync();

            return new ServiceResult<ArtistResult>
            {
                Data = new ArtistResult
                {
                    Id = artist.Id,
                    Name = artist.Name,
                    Description = artist.Description,
                    ImageResult = artist.Image != null
                                  ? new ImageResult
                                  {
                                      Alt = artist.Image.Alt,
                                      ContentType = artist.Image.ContentType,
                                      Created = artist.Image.Created,
                                      Updated = artist.Image.Updated,
                                      Id = artist.Image.Id
                                  }
                                  : null
                }
            };
        }
    }
}