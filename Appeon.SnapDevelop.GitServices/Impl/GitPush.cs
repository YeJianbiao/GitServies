using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System.Collections.Generic;
using System.Linq;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitPush
    {
        public void DoPush()
        {
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                var options = new PushOptions
                {
                    CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = GitConstants.Account,
                        Password = GitConstants.Password
                    })
                };
                repo.Network.Push(repo.Head, options);
            }
        }

        public void DoPush(string branch)
        {
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                var options = new PushOptions
                {
                    CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = GitConstants.Account,
                        Password = GitConstants.Password
                    })
                };
                repo.Network.Push(repo.Branches[branch], options);
            }
        }

        public void DoPushChanges()
        {
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                foreach (Remote remote in repo.Network.Remotes)
                {
                    var options = new PushOptions
                    {
                        CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                       new UsernamePasswordCredentials()
                       {
                           Username = GitConstants.Account,
                           Password = GitConstants.Password
                       })
                    };
                    IEnumerable<string> refSpecs = remote.PushRefSpecs.Select(x => x.Specification);
                    //var pushRefSpec = @"refs/heads/master";
                    repo.Network.Push(remote, refSpecs, options);
                }
            }

        }

    }
}
