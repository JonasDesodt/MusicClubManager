using System.Globalization;
using System.Windows.Data;

namespace MusicClubManager.Cms.Wpf.Converters
{
    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int month)
            {
                return CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month);

           //     return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            }

            return "undefined";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
