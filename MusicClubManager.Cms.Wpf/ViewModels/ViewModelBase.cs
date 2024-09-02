using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isReady = true;
        public bool IsReady
        {
            get => _isReady;
            set => SetProperty(ref _isReady, value);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(backingField, value))
            {
                return false;
            }

            backingField = value;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
