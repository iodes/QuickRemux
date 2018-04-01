using System;
using System.IO;

namespace QuickRemux.Converters
{
    public class PathToNameConverter : BaseValueConverter<string, string>
    {
        public override string Convert(string value, object parameter)
        {
            return Path.GetFileName(value);
        }

        public override string ConvertBack(string value, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
