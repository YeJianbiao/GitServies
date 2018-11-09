using System.Windows.Forms;

namespace Appeon.SnapDevelop.GitServices
{
    public interface IGitCommand
    {

        GitExecuteResult Add(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Branch(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Checkout(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Clone(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Commit(GitFileArgs args, MethodInvoker action);

        GitExecuteResult CommitTree(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Diff(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Fetch(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Init(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Log(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Pull(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Push(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Merge(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Remove(GitFileArgs args, MethodInvoker action);

        GitExecuteResult Revert(GitFileArgs args, MethodInvoker action);


    }
}
