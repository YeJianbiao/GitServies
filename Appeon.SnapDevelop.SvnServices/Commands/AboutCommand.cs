using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    internal class AboutCommand : SubversionCommand
    {
        public override SvnExecuteResult Run(SvnFileArgs ar)
        {
            SvnCommand.ShowSvnAbout();
            return default(SvnExecuteResult);
        }
    }
}
