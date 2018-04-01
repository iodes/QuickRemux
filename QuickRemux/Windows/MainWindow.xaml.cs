using Microsoft.Win32;
using QuickRemux.Engine;
using QuickRemux.Supports;
using QuickRemux.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;

namespace QuickRemux
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        #region 변수
        SemaphoreSlim _semaphore;
        #endregion

        #region 속성
        public ObservableCollection<MKVRemuxer> Remuxers { get; } = new ObservableCollection<MKVRemuxer>();
        #endregion

        #region 생성자
        public MainWindow()
        {
            InitializeComponent();
            SetSemaphoreCount(ProcessorSupport.GetAvailableCount());
            DataContext = this;
        }
        #endregion

        #region 이벤트
        private void ListWork_Drop(object sender, DragEventArgs e)
        {
            Enqueue(e.Data.GetData(DataFormats.FileDrop) as string[]);
        }

        private void ContextAdd_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "MKV 파일|*.mkv|모든 파일|*.*"
            };

            if (openDialog.ShowDialog() == true)
            {
                Enqueue(openDialog.FileNames);
            }
        }

        private void ContextClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }
        #endregion

        #region 내부 함수
        private void Clear()
        {
            foreach (var remuxer in Remuxers.Where(x => x.Status != RemuxerStatus.Standby && x.Status != RemuxerStatus.Running).ToList())
            {
                Remuxers.Remove(remuxer);
            }
        }

        private void Enqueue(string[] targets)
        {
            foreach (var file in targets.Where(x => x.EndsWith(".mkv")))
            {
                var mkvRemuxer = new MKVRemuxer
                {
                    Input = file,
                    Output = $"{file}.mp4"
                };

                Remuxers.Add(mkvRemuxer);

                var thread = new Thread(() =>
                {
                    _semaphore.Wait();
                    mkvRemuxer.Start();
                    _semaphore.Release();
                });

                thread.Start();
            }
        }

        private void SetSemaphoreCount(int value)
        {
            _semaphore = new SemaphoreSlim(value);
        }
        #endregion
    }
}
