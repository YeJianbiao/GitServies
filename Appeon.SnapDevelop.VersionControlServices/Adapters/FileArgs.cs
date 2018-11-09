using System;
using System.IO;

namespace Appeon.SnapDevelop.VersionControlServices
{
    public abstract class FileArgs : EventArgs
    {
        public string FileName { get; set; }

        public bool IsDirectory { get; set; }

        public Uri From { get; set; }
        public Uri To { get; set; }

        public long VersionStart { get; set; }
        public long VersionEnd { get; set; }

        public string Comment { get; set; }
    }

   
}
