using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    internal class ExportWorkingCopyCommand : SubversionCommand
    {
        public override SvnExecuteResult Run(SvnFileArgs args)
        {
          return  SvnCommand.Export(args, null);
        }
    }
    
}
