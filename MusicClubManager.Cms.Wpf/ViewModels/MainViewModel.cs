using Microsoft.Extensions.DependencyInjection;
using MusicClubManager.Cms.Wpf.Commands;
using MusicClubManager.Cms.Wpf.Interfaces;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase, INavigate
    {
        private readonly IServiceProvider _serviceProvider;

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _currentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();

            NavigateCommand = new NavigateCommand(this);
        }

        private object? _currentViewModel;
        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value); 
        }

        public NavigateCommand NavigateCommand { get; set; }

        public void Navigate(string route)
        {
            switch (route)
            {
                case "Agenda":
                    CurrentViewModel = _serviceProvider.GetRequiredService<AgendaViewModel>();
                    break;
                case "Artists":
                    CurrentViewModel = _serviceProvider.GetRequiredService<ArtistsViewModel>();
                    break;
                case "Home":
                default:
                    CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();
                    break;
            }           
        }
    }
}
