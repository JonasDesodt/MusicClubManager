using System.Globalization;
using System.Net.Http;
using System.Windows.Data;

namespace MusicClubManager.Cms.Wpf.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is not int id)
            {
                return "https://localhost:7188/image/download/0"; //todo: have a fallback image
            }

            return "https://localhost:7188/image/download/" + id; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
