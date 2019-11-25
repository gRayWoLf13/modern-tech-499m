using System;
using System.Globalization;
using System.Windows.Data;

namespace modern_tech_499m.Converters
{
    class UserToPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Repositories.Core.Domain.User user)
            {
                switch (parameter?.ToString())
                {
                    case "Id": return $"Id : {user.Id}";
                    case "FullName": return $"Name : {user.FullName}";
                    case "BirthDate": return $"Birth date : {user.BirthDate:dd MMMM yyyy}";
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
