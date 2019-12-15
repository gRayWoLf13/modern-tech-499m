using System;
using System.Diagnostics;
using System.Globalization;
using modern_tech_499m.Pages;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Converters
{
    /// <summary>
    /// Converts a string name to a service pulled from IoC container
    /// </summary>
    class IoCConverter : BaseConverter<IoCConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Find the appropriate 
            switch ((string)value)
            {
                case nameof(ApplicationViewModel):
                    return BootStrapper.Resolve<ApplicationViewModel>();

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
