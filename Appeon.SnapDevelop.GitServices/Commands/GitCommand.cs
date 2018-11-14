
namespace Appeon.SnapDevelop.GitServices
{
    public class GitCommand
    {

        public IGitCommand Command
        {
            get
            {
                return new GitWrapper();
            }
        }

        public virtual void CallbackInvoked() { }

        public virtual void Run() { }

        public virtual void Run(GitFileArgs args) { }

    }
}
