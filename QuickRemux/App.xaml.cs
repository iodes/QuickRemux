using QuickRemux.Supports;
using System.IO;
using System.Reflection;
using System.Windows;

namespace QuickRemux
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private const string resourceEngine = "QuickRemux.Resources.FFMPEG.exe";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Install Dependencies
            if (!File.Exists(EnvironmentSupport.Engine))
            {
                using (var stream = File.Create(EnvironmentSupport.Engine))
                {
                    Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceEngine).CopyTo(stream);
                }
            }
        }
    }
}
