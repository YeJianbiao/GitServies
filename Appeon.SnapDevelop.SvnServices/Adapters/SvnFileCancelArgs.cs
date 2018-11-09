using System;
using Appeon.SnapDevelop.VersionControlServices;

namespace Appeon.SnapDevelop.SvnServices
{    

    public class SvnFileCancelArgs : FileCancelArgs
    {     
        public bool IsForce { get; set; }
        public SvnFileCancelArgs( )
        {

        }
        public SvnFileCancelArgs(string fileName, bool isDirectory=false)  
        {
            this.FileName = fileName;
            this.IsDirectory = isDirectory;
        }
    }
}
