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
                foreach (var branch in repo.Branches)
                {
                    Console.WriteLine(branch);
                }
            }
            Console.ReadKey();
        }

        void Add()
        {
            using (var repo = new Repository(path))
            {
                //暂存单个文件的添加，一般执行后就会有右键git菜单（提交|撤销）
                repo.Index.Add("Appeon.SnapDevelop.GitServices/Commands/CloneCommand.cs");
                repo.Index.Write();

                //暂存当前目录下的所有文件，参数表示过滤，*代表全部
                Commands.Stage(repo, "*");
            }
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
