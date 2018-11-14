using LibGit2Sharp;
using System.IO;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitClone : GitBase
    {

        internal static bool Clone(string retotePath,string localPath)
        {

            string clonedRepoPath = Repository.Clone(retotePath, localPath);

            using (var repo = new Repository(clonedRepoPath))
            {
                string dir = repo.Info.Path;
                if (!Path.IsPathRooted(dir)||!Directory.Exists(dir)||string.IsNullOrEmpty(repo.Info.WorkingDirectory)|| !Path.Combine(clonedRepoPath, ".git" + Path.DirectorySeparatorChar).Equals(repo.Info.Path)|| repo.Info.IsBare||!File.Exists(Path.Combine(clonedRepoPath, "master.txt"))|| "master".Equals(repo.Head.FriendlyName)||!"49322bb17d3acc9146f98c97d078513228bbf3c0".Equals(repo.Head.Tip.Id.ToString()))
                {
                    return false;
                }
            }
            return true;
        }
    }

}
