using System;
using System.Diagnostics;
using System.Globalization;
using modern_tech_499m.Pages;

namespace modern_tech_499m.Converters
{
    class PageValueConverter : BaseConverter<PageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Login:
                    return new LoginPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
