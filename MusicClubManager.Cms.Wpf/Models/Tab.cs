using MusicClubManager.Cms.Wpf.Commands;

namespace MusicClubManager.Cms.Wpf.Models
{
    public class Tab
    {
        public required string Header { get; set; }

        public CloseTabCommand CloseTabCommand { get; set; }

        public object? ViewModel { get; set; }

        public Tab(object? viewModel)
        {
            CloseTabCommand = new CloseTabCommand(this);

            ViewModel = viewModel;
        }
    }
}
 