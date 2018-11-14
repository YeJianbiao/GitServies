using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitCheckout : GitBase
    {

        internal static bool CheckoutBranch(string rootPath,string branchName)
        {
            using (var repo = new Repository(rootPath))
            {
                Branch master = repo.Branches["master"];
                if (!master.IsCurrentRepositoryHead){
                    return false;
                }
                // Set the working directory to the current head
                ResetAndCleanWorkingDirectory(repo);

                if (repo.RetrieveStatus().IsDirty){
                    return false;
                }

                Branch branch = repo.Branches[branchName];
                if (branch == null)
                {
                    return false;
                }
                if(!AssertBelongsToARepository(repo, branch)){
                    return true;
                }
                return true;
                //Branch test = Commands.Checkout(repo, branch);
                //Assert.False(repo.Info.IsHeadDetached);
                //AssertBelongsToARepository(repo, test);

                //Assert.False(test.IsRemote);
                //Assert.True(test.IsCurrentRepositoryHead);
                //Assert.Equal(repo.Head, test);

                //Assert.False(master.IsCurrentRepositoryHead);

                //// Working directory should not be dirty
                //Assert.False(repo.RetrieveStatus().IsDirty);

                //// Assert reflog entry is created
                //var reflogEntry = repo.Refs.Log(repo.Refs.Head).First();
                //Assert.Equal(master.Tip.Id, reflogEntry.From);
                //Assert.Equal(branch.Tip.Id, reflogEntry.To);
                //Assert.NotNull(reflogEntry.Committer.Email);
                //Assert.NotNull(reflogEntry.Committer.Name);
                //Assert.Equal(string.Format("checkout: moving from master to {0}", branchName), reflogEntry.Message);
            }
        }

        /// <summary>
        /// Reset and clean current working directory. This will ensure that the current
        /// working directory matches the current Head commit.
        /// </summary>
        /// <param name="repo">Repository whose current working directory should be operated on.</param>
        private static void ResetAndCleanWorkingDirectory(IRepository repo)
        {
            // Reset the index and the working tree.
            repo.Reset(ResetMode.Hard);

            // Clean the working directory.
            repo.RemoveUntrackedFiles();
        }
    }
}
