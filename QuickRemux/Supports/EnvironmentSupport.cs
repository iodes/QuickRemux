using System;
using System.IO;

namespace QuickRemux.Supports
{
    public static class EnvironmentSupport
    {
        #region 상수
        const string engine = "FFMPEG.exe";
        #endregion

        #region 변수
        static string _storage = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\QuickRemux";
        #endregion

        #region 속성
        public static string Storage
        {
            get
            {
                if (!Directory.Exists(_storage))
                {
                    Directory.CreateDirectory(_storage);
                }

                return _storage;
            }
        }

        public static string Engine => Path.Combine(Storage, engine);
        #endregion
    }
}
