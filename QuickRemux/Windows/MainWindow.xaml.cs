using QuickRemux.Engine;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QuickRemux
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private MKVRemuxer Remuxer { get; } = new MKVRemuxer();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = Remuxer;

            Loaded += MainWindow_LoadedAsync;
            Remuxer.PropertyChanged += Remuxer_PropertyChanged;
        }

        private void Remuxer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (Remuxer.Status)
            {
                case RemuxerStatus.Failed:
                    MessageBox.Show($"{Path.GetFileName(Remuxer.Input)}\n변환 오류가 발생했습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private async void MainWindow_LoadedAsync(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                var mkvFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.mkv", SearchOption.TopDirectoryOnly);
                var workQeueue = new Queue<string>(mkvFiles);

                int count = 0;
                while (workQeueue.Any())
                {
                    var target = workQeueue.Dequeue();
                    Dispatcher.Invoke(() => Title = $"[{++count}/{mkvFiles.Count()}] {Path.GetFileName(target)}");

                    Remuxer.Input = target;
                    Remuxer.Output = $"{target}.mp4";
                    Remuxer.Start();
                }
            });

            MessageBox.Show("작업을 완료했습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Shutdown();
        }
    }
}
