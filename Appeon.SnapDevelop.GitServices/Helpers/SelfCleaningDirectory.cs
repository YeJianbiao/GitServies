using Appeon.SnapDevelop.GitServices.Impl;
using System;
using System.IO;

namespace Appeon.SnapDevelop.GitServices.Helpers
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
            return Path.Combine(GitConstants.TemporaryReposPath, Path.GetRandomFileName());
        }
    }
}
