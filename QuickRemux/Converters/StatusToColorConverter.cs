using QuickRemux.Engine;
using System;
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
                    return converter.ConvertFromString("#00000000") as Brush;

                case RemuxerStatus.Running:
                    return converter.ConvertFromString("#5EA6D4") as Brush;

                case RemuxerStatus.Succeed:
                    return converter.ConvertFromString("#50B92D") as Brush;

                case RemuxerStatus.Failed:
                    return converter.ConvertFromString("#E36543") as Brush;
            }
        }

        public override RemuxerStatus ConvertBack(Brush value, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
