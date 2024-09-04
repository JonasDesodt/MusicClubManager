using System.Windows.Data;
using System.Globalization;
using MusicClubManager.Cms.Wpf.Interfaces;
using System.Windows;

namespace MusicClubManager.Cms.Wpf.Converters
{
    public class ITabContentToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is ITabContent)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
