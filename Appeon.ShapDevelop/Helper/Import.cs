using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Appeon.ShapDevelop.Helper
{
    public class Import
    {

        public static bool ImportProjectToGithub()
        {
            CopyGitignore();
            CopyGitattributes();
            //string localRepoPath = Repository.Clone(Config.RemotePath, Config.ProjectPath);
            //string localRepoPath = Repository.Init(Config.ProjectPath, false);
            using (var localRepo = new Repository(Config.ProjectPath, new RepositoryOptions { Identity = Config.Identity }))
            {
                //Remote remote = localRepo.Network.Remotes.Add("origin", Config.RemotePath);

                Commands.Stage(localRepo,"*");
                localRepo.Commit("commit", Config.Signature, Config.Signature);


                //localRepo.Branches.Update(localRepo.Head,
                //    b => b.Remote = remote.Name,
                //    b => b.UpstreamBranch = localRepo.Head.CanonicalName);

                var options = new PushOptions();
                options.CredentialsProvider = (_url, _user, _cred) =>
                    new UsernamePasswordCredentials { Username = "admin", Password = "admin123" };
                localRepo.Network.Push(localRepo.Head, options);
                //AssertRemoteHeadTipEquals(localRepo, first.Sha);

                //UpdateTheRemoteRepositoryWithANewCommit(remoteRepoPath);

                //// Add another commit
                //var oldId = localRepo.Head.Tip.Id;
                //Commit second = AddCommitToRepo(localRepo);

                //// Try to fast forward push this new commit
                //Assert.Throws<NonFastForwardException>(() => localRepo.Network.Push(localRepo.Head));

                //// Force push the new commit
                //string pushRefSpec = string.Format("+{0}:{0}", localRepo.Head.CanonicalName);

                //var before = DateTimeOffset.Now.TruncateMilliseconds();

                //localRepo.Network.Push(localRepo.Network.Remotes.Single(), pushRefSpec);

                //AssertRemoteHeadTipEquals(localRepo, second.Sha);

                //AssertRefLogEntry(localRepo, "refs/remotes/origin/master",
                //    "update by push",
                //    oldId, localRepo.Head.Tip.Id,
                //    Constants.Identity, before);
            }

            return true;
        }

        private static void CopyGitignore()
        {
            string sourceFile = Path.Combine(Environment.CurrentDirectory, ".gitignore");
            string destinationFile = Path.Combine(Config.ProjectPath, ".gitignore");
            if (!File.Exists(destinationFile)&&File.Exists(sourceFile))
            {
                File.Copy(sourceFile, destinationFile);
            }
        }

        private static void CopyGitattributes()
        {
            string sourceFile = Path.Combine(Environment.CurrentDirectory, ".gitattributes");
            string destinationFile = Path.Combine(Config.ProjectPath, ".gitattributes");
            if (!File.Exists(destinationFile) && File.Exists(sourceFile))
            {
                File.Copy(sourceFile, destinationFile, false);
            }
        }

    }
}
