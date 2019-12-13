using System;
using System.Globalization;
using NLog;

namespace modern_tech_499m.Converters
{
    internal class GameControllerToCommandParametersConverter : BaseMultiConverter<GameControllerToCommandParametersConverter>
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            _logger.Debug("Game controller to command parameters conversion called");
            return values.Clone();
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var exception = new NotSupportedException();
            _logger.Fatal(exception, "Game controller to command parameters reverse conversion called");
            throw exception;
        }
    }
}
