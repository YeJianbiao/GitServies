using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Appeon.ShapDevelop.Func
{
    public class Ignore
    {

        public static void Init()
        {
            string repoPath = Repository.Init(Config.ProjectPath, false);
            using (var repo = new Repository(repoPath, new RepositoryOptions { Identity = Config.Identity }))
            {
                
                Touch(repo.Info.WorkingDirectory, ".gitignore", ".vs/");
                repo.Ignore.AddTemporaryRules(new[] { "*.cs" });
                //Assert.False(repo.Ignore.IsPathIgnored("File.txt"));
                //Assert.True(repo.Ignore.IsPathIgnored("NewFolder"));
                //Assert.True(repo.Ignore.IsPathIgnored(string.Join("/", "NewFolder", "NewFolder")));
                //Assert.True(repo.Ignore.IsPathIgnored(string.Join("/", "NewFolder", "NewFolder", "File.txt")));


            }
        }

        public static void AddIgnore()
        {

        }

        protected static string Touch(string parent, string file, string content = null, Encoding encoding = null)
        {
            string filePath = Path.Combine(parent, file);
            string dir = Path.GetDirectoryName(filePath);
            if (dir == null) { return ""; }
            var newFile = !File.Exists(filePath);

            Directory.CreateDirectory(dir);

            File.WriteAllText(filePath, content ?? string.Empty, encoding ?? Encoding.ASCII);

            //Workaround for .NET Core 1.x behavior where all newly created files have execute permissions set.
            //https://github.com/dotnet/corefx/issues/13342
            if (newFile)
            {
                //RemoveExecutePermissions(filePath, newFile);
            }

            return filePath;
        }

    }
}
