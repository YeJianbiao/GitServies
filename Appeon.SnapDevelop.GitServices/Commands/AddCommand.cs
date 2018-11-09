using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.GitServices.Commands
{
    public class AddCommand : GitCommand
    {

        public override void Run(GitFileArgs args)
        {
            base.Command.Add(args,null);
        }

    }
}
