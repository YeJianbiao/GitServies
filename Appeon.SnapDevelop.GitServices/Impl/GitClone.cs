using LibGit2Sharp;
using System.IO;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitClone : GitBase
    {

        internal static bool Clone(string remotePath,string projectPath)
        {

            string clonedRepoPath = Repository.Clone(remotePath, projectPath);

            using (var repo = new Repository(clonedRepoPath))
            {
                string dir = repo.Info.Path;
                if (!Path.IsPathRooted(dir)||!Directory.Exists(dir)||string.IsNullOrEmpty(repo.Info.WorkingDirectory)|| !Path.Combine(clonedRepoPath, ".git" + Path.DirectorySeparatorChar).Equals(repo.Info.Path)|| repo.Info.IsBare||!File.Exists(Path.Combine(clonedRepoPath, "master.txt"))|| "master".Equals(repo.Head.FriendlyName))
                {
                    return false;
                }
            }
            return true;
        }
    }

}
