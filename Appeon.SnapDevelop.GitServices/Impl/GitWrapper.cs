using LibGit2Sharp;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Appeon.SnapDevelop.GitServices
{
    public class GitWrapper : IGitCommand, IDisposable
    {

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public GitExecuteResult Add(GitFileArgs args, MethodInvoker action)
        {

             
            throw new NotImplementedException();
        }

        public GitExecuteResult Branch(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Checkout(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Clone(GitFileArgs args, MethodInvoker action)
        {
            string clonedRepoPath = Repository.Clone(args.From.AbsolutePath, args.To.AbsolutePath);
            bool flag = true;
            using (var repo = new Repository(clonedRepoPath))
            {
                string dir = repo.Info.Path;
                
                if (!Path.IsPathRooted(dir)|| !Directory.Exists(dir)||string.IsNullOrEmpty(repo.Info.WorkingDirectory)|| !Path.Combine(clonedRepoPath, ".git" + Path.DirectorySeparatorChar).Equals(repo.Info.Path)|| repo.Info.IsBare==true|| !repo.Head.FriendlyName.Equals("master")||!"49322bb17d3acc9146f98c97d078513228bbf3c0".Equals("repo.Head.Tip.Id.ToString()")||!File.Exists(Path.Combine(clonedRepoPath, "master.txt")))
                {
                    flag = false;
                }
            }
            return new GitExecuteResult() {IsSuccess = flag };
        }

        public GitExecuteResult Commit(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult CommitTree(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Diff(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Fetch(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Init(GitFileArgs args, MethodInvoker action)
        {
           

            throw new NotImplementedException();
        }

        public GitExecuteResult Log(GitFileArgs args, MethodInvoker action)
        {
            using (var repo = new Repository(args.RootPath))
            {
                var RFC2822Format = "ddd dd MMM HH:mm:ss yyyy K";

                foreach (Commit c in repo.Commits.Take(15))
                {
                    Console.WriteLine(string.Format("commit {0}", c.Id));

                    if (c.Parents.Count() > 1)
                    {
                        Console.WriteLine("Merge: {0}",
                            string.Join(" ", c.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray()));
                    }

                    Console.WriteLine(string.Format("Author: {0} <{1}>", c.Author.Name, c.Author.Email));
                    Console.WriteLine("Date:   {0}", c.Author.When.ToString(RFC2822Format, CultureInfo.InvariantCulture));
                    Console.WriteLine();
                    Console.WriteLine(c.Message);
                    Console.WriteLine();
                }
            }
            return new GitExecuteResult();
        }

        public GitExecuteResult Pull(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Push(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Merge(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Remove(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Revert(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
