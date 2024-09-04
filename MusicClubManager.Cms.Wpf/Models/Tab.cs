using MusicClubManager.Cms.Wpf.Commands;
using MusicClubManager.Cms.Wpf.Interfaces;

namespace MusicClubManager.Cms.Wpf.Models
{
    public class Tab
    {
        public required string Header { get; set; }

        public CloseTabCommand CloseTabCommand { get; set; }

        public ShowTabContentCommand ShowTabContentCommand { get; set; }

        public ITabContent ViewModel { get; set; }

        public Tab(ITabContent viewModel)
        {
            CloseTabCommand = new CloseTabCommand(this);
            ShowTabContentCommand = new ShowTabContentCommand(this);

            ViewModel = viewModel;
        }
    }
}
 