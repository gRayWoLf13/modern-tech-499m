using System;
using System.Globalization;
using System.Windows;

namespace modern_tech_499m.Converters
{
    class AttachedPropertyValueConverter : BaseConverter<AttachedPropertyValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? Visibility.Hidden : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
