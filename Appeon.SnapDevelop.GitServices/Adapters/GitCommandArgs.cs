using Appeon.SnapDevelop.VersionControlServices;
using System;

namespace Appeon.SnapDevelop.GitServices.Adapters
{
    public class GitCommandArgs : CommandArgs
    {

        public GitCommandArgs(VersionControlType vControlType, CommandType cmdType, EventArgs args)
        {
            this.CmdType = cmdType;
            this.Args = args;
        }

    }
}
