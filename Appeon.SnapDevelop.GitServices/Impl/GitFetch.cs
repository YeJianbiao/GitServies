using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System.Collections.Generic;
using System.Linq;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitFetch : GitBase
    {

        internal void Fetch()
        {
            string logMessage = "";
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                FetchOptions options = new FetchOptions()
                {
                    CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials()
                        {
                            Username = GitConstants.Account,
                            Password = GitConstants.Password
                        })
                };
                foreach (Remote remote in repo.Network.Remotes)
                {
                    IEnumerable<string> refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                    LibGit2Sharp.Commands.Fetch(repo, remote.Name, refSpecs, options, logMessage);
                }
            }
        }

    }
}
