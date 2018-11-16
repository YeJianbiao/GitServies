using Appeon.ShapDevelop.Func;
using Appeon.ShapDevelop.Helper;
using Appeon.ShapDevelop.Util;
using LibGit2Sharp;
using System;

namespace Appeon.ShapDevelop
{
    class Program
    {
               
        static void Main(string[] args)
        {


            Import.ImportProjectToGithub();

            //FileWatcher.Start();
            //Console.ReadKey();
        }
        
    }
}
