using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    internal class ApplyPatchCommand : SubversionCommand
    {
        public override SvnExecuteResult Run(SvnFileArgs args)
        {
          return  SvnCommand.ApplyPatch(args, CallbackInvoked);
        }
    }
     
}
