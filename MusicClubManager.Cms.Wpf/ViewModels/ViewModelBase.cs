using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isReady = false;
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
