using System;
using System.Windows.Forms;

namespace Appeon.SnapDevelop.GitServices
{
    public class GitWrapper : IGitCommand, IDisposable
    {

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public GitExecuteResult Add(GitFileArgs args, MethodInvoker action)
        {

            throw new NotImplementedException();
        }

        public GitExecuteResult Branch(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Checkout(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Clone(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Commit(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult CommitTree(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Diff(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Fetch(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Init(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Log(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
