using System;
using System.Diagnostics;
using System.IO;

namespace Appeon.ShapDevelop.Util
{
    public class SelfCleaningDirectory
    {
        public SelfCleaningDirectory(IPostTestDirectoryRemover directoryRemover)
            : this(directoryRemover, BuildTempPath())
        {
        }

        public SelfCleaningDirectory(IPostTestDirectoryRemover directoryRemover, string path)
        {
            if (Directory.Exists(path))
            {
                throw new InvalidOperationException(string.Format("Directory '{0}' already exists.", path));
            }

            DirectoryPath = path;
            RootedDirectoryPath = Path.GetFullPath(path);

            directoryRemover.Register(DirectoryPath);
        }

        public string DirectoryPath { get; private set; }
        public string RootedDirectoryPath { get; private set; }

        protected static string BuildTempPath()
        {
            string tempPath = null;

            const string LibGit2TestPath = "AppTest";

            // We're running on .Net/Windows
            if (Environment.GetEnvironmentVariables().Contains(LibGit2TestPath))
            {
                Trace.TraceInformation("{0} environment variable detected", LibGit2TestPath);
                tempPath = Environment.GetEnvironmentVariables()[LibGit2TestPath] as String;
            }

            if (String.IsNullOrWhiteSpace(tempPath) || !Directory.Exists(tempPath))
            {
                Trace.TraceInformation("Using default test path value");
                tempPath = Path.GetTempPath();
            }

            //workaround macOS symlinking its temp folder
            if (tempPath.StartsWith("/var"))
            {
                tempPath = "/private" + tempPath;
            }

            string testWorkingDirectory = Path.Combine(tempPath, "LibGit2Sharp-TestRepos");
            Trace.TraceInformation("Test working directory set to '{0}'", testWorkingDirectory);
            return Path.Combine(testWorkingDirectory, Path.GetRandomFileName());
        }

    }
}
