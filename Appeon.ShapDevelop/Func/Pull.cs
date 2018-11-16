using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appeon.ShapDevelop.Func
{
    class Pull
    {

        public void DoPull()
        {
            using (var repo = new Repository(Config.ProjectPath))
            {
                // Credential information to fetch
                PullOptions options = new PullOptions();
                options.FetchOptions = new FetchOptions();
                options.FetchOptions.CredentialsProvider = new CredentialsHandler(
                    (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials()
                        {
                            Username = Config.Name,
                            Password = Config.Password
                        });

                // User information to create a merge commit
                var signature = new Signature(Config.Identity, DateTimeOffset.Now);

                // Pull
                Commands.Pull(repo, signature, options);
            }
        }

    }
}
