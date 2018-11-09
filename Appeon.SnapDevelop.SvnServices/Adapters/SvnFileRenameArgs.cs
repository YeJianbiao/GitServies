using System;
using System.IO;
using Appeon.SnapDevelop.VersionControlServices;



namespace Appeon.SnapDevelop.SvnServices
{
    public class SvnFileRenameArgs : FileRenameArgs
    {
        public SvnFileRenameArgs()
        {

        }

        public SvnFileRenameArgs(string sourceFile, string targetFile, bool isDirectory=false)
        {
            this.SourceFile = sourceFile;
            this.TargetFile = targetFile;
            this.IsDirectory = isDirectory;
        }
    }
}
