using MusicClubManager.Dto.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Dto.Transfer
{
    public class PaginationRequest
    {
        [Required]
        [MinPage(1)]
        public required uint Page { get; set; }

        [Required]
        [MaxPageSize(24)]
        public required uint PageSize { get; set; }
    }
}