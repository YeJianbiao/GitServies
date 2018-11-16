
using System.Threading;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal static class GitStatus
    {

        private static AutoResetEvent _autoResetEvent;

        static GitStatus()
        {
            _autoResetEvent = new AutoResetEvent(true);
            var thread = new Thread(Retrieve);
            thread.IsBackground = true;
            thread.Start();
        }

        private static void Retrieve()
        {
            while (true)
            {
                var filePath = Cache.FileChanges.Dequeue();
                Cache.UncommitChanges.Add(filePath);
                if (Cache.FileChanges.Count > 0) { continue; }
                _autoResetEvent.WaitOne();
            }
        }

        public static void Reset()
        {
            _autoResetEvent.Set();
        }

    }
}
