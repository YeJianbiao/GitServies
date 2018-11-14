using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitFetch:GitBase
    {

        internal void Fetch()
        {
            string logMessage="";
            using (var repo = new Repository(GitConstants.GitRootPath))
            {
                FetchOptions options = new FetchOptions();
                options.CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = GitConstants.User,
                        Password = GitConstants.Password
                    });

                foreach (Remote remote in repo.Network.Remotes)
                {
                    IEnumerable<string> refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                    LibGit2Sharp.Commands.Fetch(repo, remote.Name, refSpecs, options, logMessage);
                }
            }
        }

    }
}
