using Microsoft.Extensions.DependencyInjection;
using MusicClubManager.Cms.Wpf.Commands;
using MusicClubManager.Cms.Wpf.Interfaces;
using MusicClubManager.Cms.Wpf.Models;
using MusicClubManager.Dto.Result;
using System.Collections.ObjectModel;

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

            Tabs = new ObservableCollection<Tab> { new Tab(_serviceProvider.GetRequiredService<ArtistsViewModel>()) { Header = "Tab 1" }, new Tab(_serviceProvider.GetRequiredService<ArtistsViewModel>()) { Header = "Tab 2" } };     
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
                case "Calendar":
                    CurrentViewModel = _serviceProvider.GetRequiredService<CalendarViewModel>();
                    break;
                default:
                    CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();
                    break;
            }           
        }

        public ObservableCollection<Tab> Tabs { get; set; }

        public Tab? AddTab(object? item)
        {
            if(item is PerformanceResult performanceResult)
            {
                var performanceViewModel = new PerformanceViewModel { ArtistName = performanceResult.ArtistResult.Name };              

                var tab = new Tab(performanceViewModel) { Header = performanceResult.ArtistResult.Name };

                Tabs.Add(tab);

                return tab;
            }

            return null;
        }
    }
}
