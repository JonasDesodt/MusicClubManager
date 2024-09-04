using MusicClubManager.Cms.Wpf.Interfaces;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MusicClubManager.Cms.Wpf.Converters
{
    class ITabContentToForegroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is null || values[1] is null)
            {
                return new SolidColorBrush(Colors.Black);
            }

            if (values[0].GetType() == values[1].GetType() && ((ITabContent)values[0]).Id == ((ITabContent)values[1]).Id)
            {
                return new SolidColorBrush(Colors.White);
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
