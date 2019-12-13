using System;
using System.Globalization;
using System.Windows.Data;
using NLog;

namespace modern_tech_499m.Converters
{
    class GameControllerToCommandParametersConverter : IMultiValueConverter
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            _logger.Debug("Game controller to command parameters conversion called");
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var exception = new NotSupportedException();
            _logger.Fatal(exception, "Game controller to command parameters reverse conversion called");
            throw exception;
        }
    }
}
