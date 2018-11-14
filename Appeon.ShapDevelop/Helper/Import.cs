using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appeon.ShapDevelop.Helper
{
    public class Import
    {

        public static bool ImportProjectToGithub()
        {

            string remoteRepoPath = Repository.Init("ApponTest", true);

            // Create a new repository
            string localRepoPath = Repository.Init(Config.ProjectPath, false);
            using (var localRepo = new Repository(localRepoPath, new RepositoryOptions { Identity = Config.Identity }))
            {
                Commands.Stage(localRepo,"*", new StageOptions { ExplicitPathsOptions = new ExplicitPathsOptions { ShouldFailOnUnmatchedPath = false } });

                Commit first= localRepo.Commit("New commit", Config.Signature, Config.Signature);
                
                Remote remote = localRepo.Network.Remotes.Add("origin", remoteRepoPath);

                localRepo.Branches.Update(localRepo.Head,
                    b => b.Remote = remote.Name,
                    b => b.UpstreamBranch = localRepo.Head.CanonicalName);

                // Push this commit
                localRepo.Network.Push(localRepo.Head);
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

        



    }
}
