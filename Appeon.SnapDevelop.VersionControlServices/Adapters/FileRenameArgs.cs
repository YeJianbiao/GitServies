using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.VersionControlServices
{
    public abstract class FileRenameArgs : EventArgs
    {
      
        public string SourceFile { get; set; }

        public string TargetFile { get; set; }


        public bool IsDirectory { get; set; }

       
    }
}
