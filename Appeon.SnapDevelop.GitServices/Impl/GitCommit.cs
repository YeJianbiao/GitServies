using LibGit2Sharp;
using System;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitCommit : GitBase
    {

        internal void Commit(string msg)
        {
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                Signature author = new Signature(GitConstants.Identity, DateTime.Now);
                Signature committer = author;
                Commit commit = repo.Commit(msg, author, committer);
            }
        }

    }
}
