﻿
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
