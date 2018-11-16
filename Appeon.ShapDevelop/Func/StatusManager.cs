using Appeon.ShapDevelop.Util;
using LibGit2Sharp;

namespace Appeon.ShapDevelop.Func
{
    public class StatusManager
    {
        public static void UpdateFileAttribute(string file)
        {
            try
            {
                using (var repo = new Repository(Config.ProjectPath))
                {
                    var status = repo.RetrieveStatus(file);

                    if (status == FileStatus.Ignored)
                    {
                        return;
                    }
                    Cache.ProcessedPath.Add(file);

                }
            }
            catch
            {

            }
        }


    }
}
