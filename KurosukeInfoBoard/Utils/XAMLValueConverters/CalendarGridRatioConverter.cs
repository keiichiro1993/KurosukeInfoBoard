using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.Utils.XAMLValueConverters
{
    public class CalendarGridRatioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return double.Parse(value.ToString()) * (3.0 / 4.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return double.Parse(value.ToString()) * (4.0 / 3.0);
        }
    }
}
