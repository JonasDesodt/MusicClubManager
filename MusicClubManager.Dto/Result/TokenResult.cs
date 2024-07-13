using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Dto.Result
{
    public class TokenResult
    {
        public required string TokenType { get; set; }

        public required string AccessToken { get; set; }

        public required int ExpiresIn { get; set; }
    }
}
