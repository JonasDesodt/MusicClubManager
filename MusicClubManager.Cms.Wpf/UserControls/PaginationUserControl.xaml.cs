using System.Windows.Controls;
using MusicClubManager.Cms.Wpf.ViewModels;
using System.Windows.Data;
using MusicClubManager.Cms.Wpf.ValidationRules;

namespace MusicClubManager.Cms.Wpf.UserControls
{
    /// <summary>
    /// Interaction logic for PaginationUserControl.xaml
    /// </summary>
    public partial class PaginationUserControl : UserControl
    {
        public PaginationUserControl()
        {
            InitializeComponent();
            
            DataContextChanged += (sender, args) =>
            {
                ((PageValidationRule)TotalPagesBinding.ValidationRules[0]).TotalPages = ((PaginationViewModel)DataContext).TotalPages;
            };
        }  
    }
}