using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Abstractions
{
    public interface ITokenStore
    {
        Task<string> GetAccessToken();

        Task RemoveToken();
    }
}
