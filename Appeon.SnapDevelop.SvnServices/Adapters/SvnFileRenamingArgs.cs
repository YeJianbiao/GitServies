using System;
using System.IO;
using Appeon.SnapDevelop.VersionControlServices;



namespace Appeon.SnapDevelop.SvnServices
{
    public class SvnFileRenamingArgs : FileRenamingArgs
    {

        public SvnFileRenamingArgs()
        {

        }

        public SvnFileRenamingArgs(string sourceFile, string targetFile, bool isDirectory=false)            
        {
            this.SourceFile = sourceFile;
            this.TargetFile = targetFile;
            this.IsDirectory = isDirectory;
            
        }
    }
}
