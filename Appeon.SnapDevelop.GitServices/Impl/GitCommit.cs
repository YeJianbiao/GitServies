using LibGit2Sharp;
using System;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitCommit : GitBase
    {

        internal static bool Commit(string msg)
        {
            using (var repo = new Repository(GitConstants.GitRootPath))
            {
                Signature author = new Signature(GitConstants.Identity, DateTime.Now);
                Signature committer = author;
                Commit commit = repo.Commit(msg, author, committer);
            }
            return true;
        }

    }
}
