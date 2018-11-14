using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitPull
    {

        public void Pull()
        {
            using (var repo = new Repository(GitConstants.GitRootPath))
            {
                var options = new PullOptions
                {
                    FetchOptions = new FetchOptions()
                    {
                        CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials()
                        {
                            Username = GitConstants.User,
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
