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

                case ApplicationPage.Register:
                    return new RegisterPage();

                case ApplicationPage.Game:
                    return new GamePage();

                case ApplicationPage.GameInfoSelection:
                    return new GameInfoSelectionPage();

                case ApplicationPage.PlayerSelection:
                    return null;

                case ApplicationPage.UsersDatabase:
                    return null;

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
