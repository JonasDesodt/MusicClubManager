using MusicClubManager.Blazor.Interfaces;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Result;
using MusicClubManager.Blazor.Models.FormModels;
using Microsoft.AspNetCore.Components;

namespace MusicClubManager.Blazor.Models.Contexts
{
    public class PerformanceContext : IWizardContext
    {
        public DateTime? Start { get; set; }
        public int? Duration { get; set; }
        public string? Type { get; set; }


        public int? ArtistId { get; set; }

        public PaginationContext ArtistPaginationContext { get; set; } = new() { Page = 1, PageSize = 24, TotalCount = 0 };

        public ArtistFilter ArtistFilter { get; set; } = new ArtistFilter();

        public ArtistCreateEditFormModel ArtistFormModel { get; set; } = new ArtistCreateEditFormModel();

        public IList<ArtistResult>? ArtistResults { get; set; }


        public int? LineupId { get; set; }

        public PaginationContext LineupPaginationContext { get; set; } = new() { Page = 1, PageSize = 24, TotalCount = 0 };

        public LineupFilter LineupFilter { get; set; } = new LineupFilter();
        
        public LineupCreateEditFormModel LineupFormModel { get; set; } = new LineupCreateEditFormModel();

        public IList<LineupResult>? LineupResults { get; set; }


        public event EventHandler? OnPropertyChanged;

        private bool _isValid;
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;

                OnPropertyChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}