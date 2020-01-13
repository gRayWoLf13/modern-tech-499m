using System;
using System.Globalization;

namespace modern_tech_499m.Converters
{
    class FirstBoolValueToTwoValuesConverter : BaseMultiConverter<FirstBoolValueToTwoValuesConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (bool)values[0] ? values[1] : values[2];
            }
            catch
            {
                return null;
            }
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
