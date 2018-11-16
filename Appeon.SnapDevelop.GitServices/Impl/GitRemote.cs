using LibGit2Sharp;
using System.Linq;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    public class GitRemote
    {

        public bool CheckRemote(string branch)
        {
            if (string.IsNullOrEmpty(GitConstants.ProjectPath))
            {
                return false;
            }
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                if (string.IsNullOrEmpty(repo.Branches[branch].RemoteName))
                {
                    return false;
                }
            }
            return true;
        }

        public bool AddRemote(string url)
        {
            return AddRemote("origin", url,url);
        }

        public bool AddRemote(string remoteName, string url, string pushUrl)
        {
            if (string.IsNullOrEmpty(GitConstants.ProjectPath))
            {
                return false;
            }
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                repo.Network.Remotes.Add(remoteName, url);
                if (!url.Equals(pushUrl))
                {
                    repo.Network.Remotes.Update(remoteName, r => r.PushUrl = pushUrl);
                }
                Remote remote = repo.Network.Remotes[remoteName];
                return remote != null;

            }
        }

        public bool AddRemote(IRepository repo, string remoteName, string url, string pushUrl)
        {
            repo.Network.Remotes.Add(remoteName, url);
            if (!url.Equals(pushUrl))
            {
                repo.Network.Remotes.Update(remoteName, r => r.PushUrl = pushUrl);
            }
            Remote remote = repo.Network.Remotes[remoteName];
            return remote != null;
        }

        public bool DeleteRemote(IRepository repo, string remoteName)
        {
            repo.Network.Remotes.Remove(remoteName);
            Remote remote = repo.Network.Remotes[remoteName];
            return remote == null;
        }

    }
}
