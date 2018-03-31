namespace QuickRemux.Engine
{
    public interface IRemuxer
    {
        #region 속성
        string Input { get; set; }

        string Output { get; set; }

        double Progress { get; }

        RemuxerStatus Status { get; }
        #endregion

        #region 함수
        void Start();

        void Cancel();
        #endregion
    }
}
