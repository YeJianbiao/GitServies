using Appeon.ShapDevelop.Helper;
using System.IO;

namespace Appeon.ShapDevelop.Util
{
    public class FileWatcher
    {
        private static FileSystemWatcher _watcher = null;
        //////private static bool _isWatch = false;

        public static void Start()
        {
            if (!Directory.Exists(Config.ProjectPath))
            {
                throw new FileNotFoundException();
            }
            _watcher = new FileSystemWatcher(Config.ProjectPath);
            //_watcher.BeginInit();
            _watcher.Created += new FileSystemEventHandler(OnFileChanged);
            _watcher.Changed += new FileSystemEventHandler(OnFileChanged);
            _watcher.Deleted += new FileSystemEventHandler(OnFileChanged);
            _watcher.Renamed += new RenamedEventHandler(OnFileChanged);
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
            //_watcher.EndInit();
            //_isWatch = true;
        }

        public static void End()
        {
            //_isWatch = false;
            _watcher.Created -= new FileSystemEventHandler(OnFileChanged);
            _watcher.Changed -= new FileSystemEventHandler(OnFileChanged);
            _watcher.Deleted -= new FileSystemEventHandler(OnFileChanged);
            _watcher.Renamed -= new RenamedEventHandler(OnFileChanged);
            _watcher.EnableRaisingEvents = false;
            _watcher = null;
        }

        protected static void OnFileChanged(object sender, FileSystemEventArgs args)
        {
            var change = new FileChangeInfo()
            {
                Name = args.Name,
                FullPath = args.FullPath,
                ChangeType = args.ChangeType
            };
            Cache.FileChanges.Enqueue(change);

            RetrieveStatus.Reset();
        }


    }

}
