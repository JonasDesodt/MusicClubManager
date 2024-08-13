using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using MusicClubManager.Abstractions;
using MusicClubManager.Sdk;
using MusicClubManager.Cms.Wpf.ViewModels;
using System;
using System.ComponentModel;
using MusicClubManager.Cms.Wpf.Navigators;
using MusicClubManager.Cms.Wpf.Views;
using MusicClubManager.Cms.Wpf.Resources;
using System.Runtime.InteropServices.JavaScript;

namespace MusicClubManager.Cms.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //todo: use host
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<ViewModelToViewSelector>();

            services.AddTransient<AgendaView>();
            services.AddTransient<AgendaViewModel>();

            services.AddScoped<IPerformanceService, PerformanceApiService>();

            services.AddHttpClient("MusicClubManagerApi", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://localhost:7188");
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Retrieve the MainWindow
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            // Get ViewModelToViewSelector from services and add it to MainWindow resources
            var templateSelector = _serviceProvider.GetRequiredService<ViewModelToViewSelector>();
            mainWindow.Resources.Add("ViewModelToViewSelector", templateSelector);

            // Show the MainWindow
            mainWindow.Show();

            //_serviceProvider.GetService<MainWindow>()?.Show();

            base.OnStartup(e);
        }
    }
}
