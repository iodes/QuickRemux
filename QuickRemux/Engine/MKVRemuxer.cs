namespace QuickRemux.Engine
{
    public class MKVRemuxer : BaseRemuxer
    {
        public override void Start()
        {
            base.Start();
            Process($@"-y -i ""{Input}"" -c copy -strict -2 ""{Output}""");
        }
    }
}
