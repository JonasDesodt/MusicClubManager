using MusicClubManager.Abstractions;
using MusicClubManager.Dto.Transfer;
using MusicClubManager.Dto.Filters;
using MusicClubManager.Sdk;
using MusicClubManager.Cms.Wpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MusicClubManager.Cms.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            DataContext = mainViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
