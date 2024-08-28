using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MusicClubManager.Cms.Wpf.Converters
{
    internal class PerformanceEndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not DateTime start)
            {
                return string.Empty;
            }

            if (values[1] is not int duration)
            {
                return string.Empty;
            }


            return start.AddMinutes(duration).ToString("HH:mm");

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
