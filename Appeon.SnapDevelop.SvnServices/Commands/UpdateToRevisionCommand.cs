using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    internal class UpdateToRevisionCommand : SubversionCommand
    {
        public override SvnExecuteResult Run(SvnFileArgs args )
        {
            return SvnCommand.UpdateToRevision(args, CallbackInvoked );
        }
    }
    
}
