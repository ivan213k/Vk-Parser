using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfAppVkParser.ViewModels.Converters
{
    class Boolnverter : IValueConverter // інвертує булеві значення
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
