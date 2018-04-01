using System;
using System.Windows;

namespace QuickRemux.Converters
{
    public class CountToVisibilityConverter : BaseValueConverter<int, Visibility>
    {
        public override Visibility Convert(int value, object parameter)
        {
            return value > 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public override int ConvertBack(Visibility value, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
