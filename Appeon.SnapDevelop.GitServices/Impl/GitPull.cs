using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Linq;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitPull
    {

        private void DoPull()
        {
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                var options = new PullOptions
                {
                    FetchOptions = new FetchOptions()
                    {
                        CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials()
                        {
                            Username = GitConstants.Account,
                            Password = GitConstants.Password
                        })
                    }
                };

                var signature = new Signature(GitConstants.Identity, DateTimeOffset.Now);

                LibGit2Sharp.Commands.Pull(repo, signature, options);
            }
        }

    }
}
