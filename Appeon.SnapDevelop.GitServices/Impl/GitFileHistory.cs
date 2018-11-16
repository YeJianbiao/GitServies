using LibGit2Sharp;
using System.Linq;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitFileHistory
    {

        public dynamic GetHistory(string fullPath)
        {
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                return repo.Commits.QueryBy(fullPath).ToList();
            }
        }

    }
}
