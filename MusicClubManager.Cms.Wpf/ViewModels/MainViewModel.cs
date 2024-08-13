using Microsoft.Extensions.DependencyInjection;

namespace MusicClubManager.Cms.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _currentViewModel = _serviceProvider.GetRequiredService<AgendaViewModel>();
        }

        private object? _currentViewModel;
        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value); 
        }
    }
}
