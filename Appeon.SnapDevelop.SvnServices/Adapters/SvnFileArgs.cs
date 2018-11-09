using System;
using System.IO;
using Appeon.SnapDevelop.VersionControlServices;


namespace Appeon.SnapDevelop.SvnServices
{
    public class SvnFileArgs : FileArgs
    {
        

        public SvnFileArgs()
        {

        }

        public SvnFileArgs(string fileName, bool isDirectory)
        {
            this.FileName = fileName;
            this.IsDirectory = isDirectory;
        }
        public SvnFileArgs(string fileName, bool isDirectory, Uri from , Uri to )
        {
            this.FileName = fileName;
            this.IsDirectory = isDirectory;
            this.From = from;
            this.To = to;
        }
    }

   
}
