using Microsoft.AspNetCore.Components;

namespace MusicClubManager.Cms.Blazor.Models.Contexts
{
    public class MainContext
    {
        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public EventHandler? OnPropertyChanged;
    }
}
