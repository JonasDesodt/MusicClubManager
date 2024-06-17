using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Dto.Request
{
    public class SignInRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
