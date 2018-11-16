using LibGit2Sharp;
using System.Linq;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitBranch:GitBase
    {

        internal static bool CreateBranch(string name)
        {
            using (var repo = new Repository(GitConstants.ProjectPath, new RepositoryOptions { Identity = GitConstants.Identity }))
            {
                EnableRefLog(repo);

                Branch newBranch = repo.CreateBranch(name);
                if (newBranch==null||!name.Equals(newBranch.FriendlyName)|| !("refs/heads/" + name).Equals(newBranch.CanonicalName)|| newBranch.Tip==null|| repo.Branches.SingleOrDefault(p => p.FriendlyName.Normalize() == name)==null)
                {
                    return false;
                }

            }
            return true;
        }


        public bool SetDefaultRemote(string branch)
        {
            if (string.IsNullOrEmpty(GitConstants.ProjectPath))
            {
                return false;
            }
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                if (repo.Network.Remotes.Count() != 1)
                {
                    return false;
                }
                var remote = repo.Network.Remotes.SingleOrDefault();
                repo.Branches.Update(repo.Branches[branch], b => b.Remote = remote.Name,
                    b => b.UpstreamBranch = repo.Head.CanonicalName);
            }
            return true;
        }

        public bool SetRemote(string branch,string remotename)
        {
            if (string.IsNullOrEmpty(GitConstants.ProjectPath))
            {
                return false;
            }
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                var remote = repo.Network.Remotes.SingleOrDefault(o => o.Name == remotename);
                repo.Branches.Update(repo.Branches[branch], b => b.Remote = remote.Name,
                    b => b.UpstreamBranch = repo.Head.CanonicalName);
            }
            return true;
        }

    }
}
