using LibGit2Sharp;
using System;

namespace Appeon.ShapDevelop
{
    class Program
    {

        static string path = @"C:\Users\appeon\Source\Repos\GitServies";


        static void Main(string[] args)
        {
            using (var repo = new Repository(path))
            {
                Console.WriteLine(repo.Index.Count);
                repo.Index.Add("Appeon.SnapDevelop.GitServices/Commands/AddCommand.cs");
                Console.WriteLine(repo.Index.Count);
                repo.Index.Write();
                Console.WriteLine(repo.Index.Count);
                Commands.Stage(repo, "*");
                Console.WriteLine(repo.Index.Count);
            }
        }

        void Add()
        {

        }

        void GetConfig()
        {
            using (var repo = new Repository(path))
            {
                Console.WriteLine(repo.Config);
                //Assert.Null(repo.Config.Get<bool>("unittests.boolsetting"));

                //repo.Config.Set("unittests.boolsetting", true);
                //Assert.True(repo.Config.Get<bool>("unittests.boolsetting").Value);

                //repo.Config.Unset("unittests.boolsetting");

                //Assert.Null(repo.Config.Get<bool>("unittests.boolsetting"));
            }
        }
    }
}
