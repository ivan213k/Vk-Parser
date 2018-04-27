using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfAppVkParser.ViewModels.Converters
{
    class EnUaSexConverter : IValueConverter //перекладає елементи списку «Стать»
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tempvalue = value as string;
            switch (tempvalue)
            {
                case "Male": return "Чоловіча";
                case "Female": return "Жіноча";
                default: return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
