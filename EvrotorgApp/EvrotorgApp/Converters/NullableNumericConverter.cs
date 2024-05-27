using System;
using System.Globalization;
using System.Windows.Data;

namespace EvrotorgApp.Converters
{
    public class NullableNumericConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && stringValue.Length == 0)
            {
                return null;
            }

            return value;
        }
    }
}