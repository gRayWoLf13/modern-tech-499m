using System;
using System.Globalization;
using modern_tech_499m.ViewModels;
using NLog;

namespace modern_tech_499m.Converters
{
    internal class WrappedGameInfoToPropertyConverter : BaseConverter<WrappedGameInfoToPropertyConverter>
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _logger.Debug($"Wrapped game info to property conversion called with parameter {parameter}");
            if (!(value is GameInfoSelectionViewModel.GameInfoWrapper wrappedInfo))
                return null;
            switch (parameter?.ToString())
            {
                case "PlayerNames": return $"{wrappedInfo.Player1Name} vs {wrappedInfo.Player2Name}";
                case "Id": return $"Id : {wrappedInfo.GameInfo.Id}";
                case "GameFinished":
                    return wrappedInfo.GameInfo.GameFinished ? "Game was finished" : "Game was not finished";
                case "Score": return $"Score : {wrappedInfo.GameInfo.Score}";
                default: return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var exception = new NotSupportedException();
            _logger.Fatal(exception, "Wrapped game info to property reverse convertion called");
            throw exception;
        }
    }
}
