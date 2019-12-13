﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace modern_tech_499m.Converters
{
    internal abstract class BaseMultiConverter<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {
        private static T _mConverter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _mConverter ?? (_mConverter = new T());
        }
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}