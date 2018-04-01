using System;

namespace QuickRemux.Supports
{
    public static class ProcessorSupport
    {
        public static int GetAvailableCount()
        {
            return Math.Max(1, (int)Math.Ceiling((double)Environment.ProcessorCount / 3));
        }
    }
}
