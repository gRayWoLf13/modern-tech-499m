using System;
using System.Globalization;
using NLog;

namespace modern_tech_499m.Converters
{
    internal class UserToPropertyConverter : BaseConverter<UserToPropertyConverter>
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _logger.Debug($"User to property conversion called with parameter {parameter}");
            if (!(value is Repositories.Core.Domain.User user))
                return null;
            switch (parameter?.ToString())
            {
                case "Id": return $"Id : {user.Id}";
                case "FullName": return $"Name : {user.FullName}";
                case "BirthDate": return $"Birth date : {user.BirthDate:dd MMMM yyyy}";
                default: return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var exception = new NotSupportedException();
            _logger.Fatal(exception, "User to property reverse conversion called");
            throw exception;
        }
    }
}
