using QuickRemux.Supports;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace QuickRemux.Engine
{
    public abstract class BaseRemuxer : IRemuxer, INotifyPropertyChanged
    {
        #region 상수
        private const string patternTotal = @"Video:.+?NUMBER_OF_FRAMES: (\d*)";
        private const string patternCurrent = @"frame=\s*(\d+)";
        private readonly string[] errorList =
        {
            "Invalid argument",
            "No such file or directory",
            "Invalid data found when processing input"
        };
        #endregion
        
        #region 변수
        private double _progress;
        private double _currentFrame = -1;
        private double _totalFrame = -1;
        private bool _isInitialized = false;
        private RemuxerStatus _status;
        private RemuxerStatus _lastStatus;
        private StringBuilder _logBuilder = new StringBuilder();
        private Process _process;
        #endregion

        #region 속성
        public string Input { get; set; }

        public string Output { get; set; }

        public double Progress => CurrentProgress;

        protected double CurrentProgress
        {
            get => _progress;
            set
            {
                _progress = value;
                RaisePropertyChanged(nameof(Progress));
            }
        }

        public RemuxerStatus Status => CurrentStatus;

        protected RemuxerStatus CurrentStatus
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }
        #endregion

        #region 생성자
        public BaseRemuxer()
        {
            Initialize();
        }
        #endregion

        #region 가상 함수
        public virtual void Start()
        {
            if (Status != RemuxerStatus.Running)
            {
                Initialize();
                CurrentStatus = RemuxerStatus.Running;
            }
        }

        public virtual void Cancel()
        {
            if (Status == RemuxerStatus.Running)
            {
                Dispose();
                CurrentStatus = RemuxerStatus.Cancelled;
            }
        }
        #endregion

        #region 내부 함수
        private void Dispose()
        {
            if (_process != null)
            {
                if (!_process.HasExited)
                {
                    _process.Kill();
                    _process.Close();
                }

                _totalFrame = -1;
                _currentFrame = -1;
                _isInitialized = false;
                _process = null;
            }
        }

        private void Initialize()
        {
            if (!_isInitialized)
            {
                _process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = EnvironmentSupport.Engine,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardError = true
                    },

                    EnableRaisingEvents = true
                };

                _process.Exited += _process_Exited;
                _process.ErrorDataReceived += _process_ErrorDataReceived;

                CurrentProgress = 0;
                CurrentStatus = RemuxerStatus.Standby;

                _lastStatus = CurrentStatus;
                _isInitialized = true;
            }
        }
        #endregion

        #region 외부 함수
        protected void Process(string arguments)
        {
            _process.StartInfo.Arguments = arguments;

            _process.Start();
            _process.BeginErrorReadLine();
            _process.WaitForExit();
        }
        #endregion

        #region 내부 이벤트
        private void _process_Exited(object sender, EventArgs e)
        {
            CurrentStatus = _lastStatus;
            Dispose();
        }

        private void _process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
                return;

            if (!Regex.Match(e.Data, string.Join("|", errorList), RegexOptions.IgnoreCase).Success)
            {
                // Get Totale Frame
                if (_totalFrame < 0)
                {
                    _logBuilder.AppendLine(e.Data);
                    var regexTotal = Regex.Match(_logBuilder.ToString(), patternTotal, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    if (regexTotal.Success)
                    {
                        _totalFrame = double.Parse(regexTotal.Groups[1].Value);
                        _logBuilder.Clear();
                    }
                }

                // Get Current Frame
                var regexCurrent = Regex.Match(e.Data, patternCurrent, RegexOptions.IgnoreCase);
                if (regexCurrent.Success)
                {
                    _currentFrame = double.Parse(regexCurrent.Groups[1].Value);
                }

                // Update Progress
                if (_totalFrame > 0 && _currentFrame > 0)
                {
                    CurrentProgress = _currentFrame / _totalFrame * 100;
                    _lastStatus = RemuxerStatus.Succeed;
                }
            }
            else
            {
                _lastStatus = RemuxerStatus.Failed;
            }
        }
        #endregion

        #region 외부 이벤트
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                OnPropertyChanged(propertyName);
            }
        }
        #endregion
    }
}
