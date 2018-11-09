using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    internal class CommitCommand : SubversionCommand
    {
        public override SvnExecuteResult Run(SvnFileArgs args)
        {
           return SvnCommand.Commit(args, CallbackInvoked);
        }
    }
   
}
