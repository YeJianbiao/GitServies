using Appeon.ShapDevelop.Util;
using LibGit2Sharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Appeon.ShapDevelop.Helper
{
    public class RetrieveStatus
    {

        private static AutoResetEvent _autoResetEvent;

        public void DoRetrieveStatus()
        {

        }

        public static void Init()
        {
            _autoResetEvent = new AutoResetEvent(true);
            while (Cache.FileChanges.Count > 0)
            {
                var fileChange = Cache.FileChanges.Dequeue();

                if (!Cache.ProcessedPath.Contains(fileChange.FullPath))
                {
                    Func.StatusManager.UpdateFileAttribute(fileChange.FullPath);
                }

                if (Cache.FileChanges.Count == 0)
                {
                    _autoResetEvent.WaitOne();
                }
            }
            Console.WriteLine("buhuizhix");
        }

        public static void Reset()
        {
            if(_autoResetEvent == null)
            {
                Task.Run(() =>
                {
                    Init();
                });
            }
            else
            {
                if (Cache.FileChanges.Count == 1)
                {
                    _autoResetEvent.Reset();
                }
            }
        }

        

    }
}
