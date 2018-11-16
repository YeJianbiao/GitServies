using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitCheckout : GitBase
    {

        internal static bool CheckoutBranch(string branchName)
        {
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                return true;
            }
        }

    }
}
