using System.Globalization;
using System.Windows.Data;

namespace MusicClubManager.Cms.Wpf.Converters
{
    class DateOnlyToDayOfMonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
         

            if(value is DateOnly dateOnly)
            {
                return dateOnly.Day;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
