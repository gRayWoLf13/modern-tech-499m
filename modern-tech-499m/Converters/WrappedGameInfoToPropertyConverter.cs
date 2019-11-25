using System;
using System.Globalization;
using System.Windows.Data;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Converters
{
    class WrappedGameInfoToPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GameInfoSelectionViewModel.GameInfoWrapper wrappedInfo)
            {
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
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
