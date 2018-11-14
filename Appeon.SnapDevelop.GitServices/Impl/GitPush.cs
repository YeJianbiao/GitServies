using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitPush
    {

        public void PushChanges()
        {
            using (var repo = new Repository(GitConstants.GitRootPath))
            {
                foreach (Remote remote in repo.Network.Remotes)
                {
                    var options = new PushOptions
                    {
                        CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                       new UsernamePasswordCredentials()
                       {
                           Username = GitConstants.User,
                           Password = GitConstants.Password
                       })
                    };
                    var pushRefSpec = @"refs/heads/master";
                    repo.Network.Push(remote, pushRefSpec, options);
                }
            }

        }

    }
}
