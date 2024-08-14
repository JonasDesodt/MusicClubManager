using MusicClubManager.Dto.Filters;
using System.Globalization;
using System.Windows.Data;

namespace MusicClubManager.Cms.Wpf.Converters
{
    public class ArtistFilterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new ArtistFilter { Name = values[0] as string };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
