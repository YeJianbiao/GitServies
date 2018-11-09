using System;
using Appeon.SnapDevelop.VersionControlServices;

namespace Appeon.SnapDevelop.SvnServices
{
    public class SvnCommandArgs :CommandArgs 
    {

        public SvnCommandArgs(  CommandType cmdType, EventArgs args = null )
        {            
            this.CmdType = cmdType;
            this.Args = args;
        }

    }
}
