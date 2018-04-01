using QuickRemux.Engine;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace QuickRemux
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 속성
        public ObservableCollection<MKVRemuxer> Remuxers { get; } = new ObservableCollection<MKVRemuxer>();
        #endregion

        #region 생성자
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Loaded += MainWindow_LoadedAsync;
        }
        #endregion

        #region 이벤트
        private async void MainWindow_LoadedAsync(object sender, RoutedEventArgs e)
        {
            var mkvFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.mkv", SearchOption.TopDirectoryOnly);
            foreach (var file in mkvFiles)
            {
                Remuxers.Add(new MKVRemuxer
                {
                    Input = file,
                    Output = $"{file}.mp4"
                });
            }

            await Task.Run(() =>
            {
                Parallel.ForEach(Remuxers, new ParallelOptions { MaxDegreeOfParallelism = 2 }, remuxer =>
                {
                    remuxer.Start();
                });
            });

            Application.Current.Shutdown();
        }
        #endregion
    }
}
