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
                Image = request.Image
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
                    Image = artist.Image
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
            var artist = await dbContext.Artists.FindAsync(id);

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
                    Image = artist.Image
                }
            };
        }

        public async Task<PagedServiceResult<IList<ArtistResult>>> GetAll(PaginationRequest paginationRequest, ArtistFilter filter)
        {
            var totalCount = await dbContext.Artists  
                .AddFilter(filter)
                .CountAsync();
            
            var skip = (paginationRequest.Page - 1) * paginationRequest.PageSize;

            var artists = await dbContext.Artists
                .AddFilter(filter)
                .Skip((int)skip)
                .Take((int)paginationRequest.PageSize)
                .Select(a => new ArtistResult
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Image = a.Image
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
            var artist = await dbContext.Artists.FindAsync(id);

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
            artist.Image = request.Image;

            dbContext.Artists.Update(artist);

            await dbContext.SaveChangesAsync();

            return new ServiceResult<ArtistResult>
            {
                Data = new ArtistResult
                {
                    Id = artist.Id,
                    Name = artist.Name,
                    Description = artist.Description,
                    Image = artist.Image
                }
            };
        }
    }
}