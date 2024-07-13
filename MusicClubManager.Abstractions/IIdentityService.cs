using MusicClubManager.Dto.Request;
using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;
using static MusicClubManager.Dto.Request.RegisterRequest;

namespace MusicClubManager.Abstractions
{
    public interface IIdentityService
    {
        Task<ServiceResult<string>> Register(RegisterRequest registerRequest);

        Task<ServiceResult<TokenResult>> GetToken(TokenRequest tokenRequest);
    }
}