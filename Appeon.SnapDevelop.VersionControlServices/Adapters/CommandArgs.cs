using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.VersionControlServices
{
    public abstract class CommandArgs : EventArgs
    {      
        public CommandType    CmdType { get; set; }
        public EventArgs Args { get; set; }

    }
}
