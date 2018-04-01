using QuickRemux.Engine;
using System;
using System.Windows;
using System.Windows.Media;

namespace QuickRemux.Converters
{
    public class StatusToColorConverter : BaseValueConverter<RemuxerStatus, Brush>
    {
        public override Brush Convert(RemuxerStatus value, object parameter)
        {
            var converter = new BrushConverter();

            switch (value)
            {
                default:
                case RemuxerStatus.Standby:
                    return Application.Current.FindResource("Brush.Status.Standby") as Brush;

                case RemuxerStatus.Running:
                    return Application.Current.FindResource("Brush.Status.Running") as Brush;

                case RemuxerStatus.Succeed:
                    return Application.Current.FindResource("Brush.Status.Succeed") as Brush;

                case RemuxerStatus.Failed:
                    return Application.Current.FindResource("Brush.Status.Failed") as Brush;
            }
        }

        public override RemuxerStatus ConvertBack(Brush value, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
