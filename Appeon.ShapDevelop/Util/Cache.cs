using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Appeon.ShapDevelop.Util
{
    public class Cache
    {

        public static Queue<FileChangeInfo> FileChanges = new Queue<FileChangeInfo>();

        public static List<string> ProcessedPath = new List<string>();

    }

    public class FileChangeInfo
    {
        public string Name { get; set; }

        public string FullPath { get; set; }

        public WatcherChangeTypes ChangeType { get; set; }

    }

   
}
