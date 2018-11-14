using LibGit2Sharp;
using System;
using System.Linq;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitBranch:GitBase
    {

        internal static bool CreateBranch(string rootPath,string name)
        {
            using (var repo = new Repository(rootPath, new RepositoryOptions { Identity = GitConstants.Identity }))
            {
                EnableRefLog(repo);

                const string committish = "be3563ae3f795b2b4353bcce3a527ad0a4f7f644";

                Branch newBranch = repo.CreateBranch(name, committish);
                if (newBranch==null||!name.Equals(newBranch.FriendlyName)|| !("refs/heads/" + name).Equals(newBranch.CanonicalName)|| newBranch.Tip==null|| committish.Equals(newBranch.Tip.Sha)|| repo.Branches.SingleOrDefault(p => p.FriendlyName.Normalize() == name)==null)
                {
                    return false;
                }

            }
            return true;
        }

    }
}
