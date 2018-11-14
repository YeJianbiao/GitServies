using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace Appeon.ShapDevelop.Func
{
    public class Push
    {

        public static void DoPush()
        {

            using (var repo = new Repository(Config.ProjectPath))
            {
                foreach (Remote remote in repo.Network.Remotes)
                {
                    var options = new PushOptions
                    {
                        CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                       new UsernamePasswordCredentials()
                       {
                           Username = Config.UserName,
                           Password = Config.Password
                       })
                    };
                    var pushRefSpec = @"refs/heads/master";
                    repo.Network.Push(remote, pushRefSpec, options);
                }
            }
        }
        
    }

}
