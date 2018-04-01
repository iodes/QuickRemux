using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace QuickRemux.Converters
{
    public abstract class BaseValueConverter<TFrom, TTo> : MarkupExtension, IValueConverter
    {
        public abstract TTo Convert(TFrom value, object parameter);

        public abstract TFrom ConvertBack(TTo value, object parameter);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((TFrom)value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack((TTo)value, parameter);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
