
using System.IO;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class FileWatcher
    {

        private static FileSystemWatcher _watcher = null;
        private static bool _isWatch = false;

        public static void Start()
        {
            if (!Directory.Exists(GitConstants.ProjectPath))
            {
                throw new FileNotFoundException();
            }
            if (_isWatch) { return; }
            _watcher = new FileSystemWatcher(GitConstants.ProjectPath);
            _watcher.Created += new FileSystemEventHandler(OnFileChanged);
            _watcher.Changed += new FileSystemEventHandler(OnFileChanged);
            _watcher.Deleted += new FileSystemEventHandler(OnFileChanged);
            _watcher.Renamed += new RenamedEventHandler(OnFileChanged);
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
            _isWatch = true;
        }

        public static void End()
        {
            if (_isWatch == false || _watcher == null) { return; }
            _watcher.Created -= new FileSystemEventHandler(OnFileChanged);
            _watcher.Changed -= new FileSystemEventHandler(OnFileChanged);
            _watcher.Deleted -= new FileSystemEventHandler(OnFileChanged);
            _watcher.Renamed -= new RenamedEventHandler(OnFileChanged);
            _watcher.EnableRaisingEvents = false;
            _watcher = null;
            _isWatch = false;
        }

        protected static void OnFileChanged(object sender, FileSystemEventArgs args)
        {


        }

    }
}
