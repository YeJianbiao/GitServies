using LibGit2Sharp;
using System;
using System.Linq;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitLog
    {

        public void GetLog()
        {
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                foreach (Commit c in repo.Commits)
                {
                    Console.WriteLine(string.Format("commit {0}", c.Id));

                    if (c.Parents.Count() > 1)
                    {
                        Console.WriteLine("Merge: {0}",
                            string.Join(" ", c.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray()));
                    }

                    Console.WriteLine(string.Format("Author: {0} <{1}>", c.Author.Name, c.Author.Email));
                    Console.WriteLine();
                    Console.WriteLine(c.Message);
                    Console.WriteLine();
                }
            }

        }


    }
}
