using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitAdd:GitBase
    {

        internal static void Add(string filePath)
        {
            using(var repo = new Repository(GitConstants.ProjectPath))
            {
                repo.Index.Add(filePath);
                repo.Index.Write();
            }
        }

        public void StageChanges()
        {
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                RepositoryStatus status = repo.RetrieveStatus();
                List<string> filePaths = status.Modified.Select(mods => mods.FilePath).ToList();
                LibGit2Sharp.Commands.Stage(repo, filePaths);
            }
            
        }

        public void AddSolutionToSubverion()
        {
            CopyGitignore();
            CopyGitattributes();
            string localRepoPath = Repository.Init(GitConstants.ProjectPath, false);
            using (var localRepo = new Repository(localRepoPath, new RepositoryOptions { Identity = GitConstants.Identity }))
            {
                LibGit2Sharp.Commands.Stage(localRepo, "*");
                localRepo.Commit("Add project file", new Signature(GitConstants.Identity,DateTime.Now),new Signature(GitConstants.Identity,DateTime.Now));

                //localRepo.Branches.Update(localRepo.Head,
                //    b => b.Remote = remote.Name,
                //    b => b.UpstreamBranch = localRepo.Head.CanonicalName);

                //var options = new PushOptions();
                //options.CredentialsProvider = (_url, _user, _cred) =>
                //    new UsernamePasswordCredentials { Username = "admin", Password = "admin123" };
                //localRepo.Network.Push(localRepo.Head, options);
            }

        }

        private static void CopyGitignore()
        {
            string sourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".gitignore");
            string destinationFile = Path.Combine(GitConstants.ProjectPath, ".gitignore");
            if (!File.Exists(destinationFile) && File.Exists(sourceFile))
            {
                File.Copy(sourceFile, destinationFile);
            }
        }

        private static void CopyGitattributes()
        {
            string sourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".gitattributes");
            string destinationFile = Path.Combine(GitConstants.ProjectPath, ".gitattributes");
            if (!File.Exists(destinationFile) && File.Exists(sourceFile))
            {
                File.Copy(sourceFile, destinationFile, false);
            }
        }

    }
}
