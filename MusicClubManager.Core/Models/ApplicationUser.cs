using Microsoft.AspNetCore.Identity;

namespace MusicClubManager.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}