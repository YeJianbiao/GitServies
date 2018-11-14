using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitAdd:GitBase
    {

        internal static void Add(string filePath)
        {
            using(var repo = new Repository(GitConstants.GitRootPath))
            {
                repo.Index.Add(filePath);
                repo.Index.Write();
            }
        }

        public void StageChanges()
        {
            using (var repo = new Repository(GitConstants.GitRootPath))
            {
                RepositoryStatus status = repo.RetrieveStatus();
                List<string> filePaths = status.Modified.Select(mods => mods.FilePath).ToList();
                LibGit2Sharp.Commands.Stage(repo, filePaths);
            }
            
        }

    }
}
