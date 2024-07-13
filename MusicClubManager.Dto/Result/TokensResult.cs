namespace MusicClubManager.Dto.Result 
{ 
    public class TokensResult
    {
        public required string TokenType { get; set; }

        public required string AccessToken { get; set; }

        public required int ExpiresIn { get; set; }

        public required string RefreshToken { get; set; }


        public DateTime Received { get; set; } = DateTime.UtcNow;
    }
}