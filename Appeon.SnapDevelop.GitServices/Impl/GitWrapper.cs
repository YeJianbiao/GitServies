using Appeon.SnapDevelop.GitServices.Impl;
using LibGit2Sharp;
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

        /// <summary>
        /// 添加到提交列表
        /// </summary>
        /// <param name="args"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public GitExecuteResult Add(GitFileArgs args, MethodInvoker action)
        {
            try
            {
                GitAdd.Add(args.RootPath);
                return new GitExecuteResult() { };
            }
            catch (Exception ex)
            {
                return new GitExecuteResult() { IsSuccess = false, Exp = ex };
            }
        }

        /// <summary>
        /// 创建一个分支
        /// </summary>
        /// <param name="args"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public GitExecuteResult Branch(GitFileArgs args, MethodInvoker action)
        {
            try
            {
                bool flag = GitBranch.CreateBranch("Branch");
                if (flag)
                {
                    return new GitExecuteResult() {IsSuccess = true };
                }
                else
                {
                    return new GitExecuteResult() { IsSuccess = false, Message = "" };
                }
            }
            catch (Exception ex)
            {
                return new GitExecuteResult() { IsSuccess = false,Exp = ex };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public GitExecuteResult Checkout(GitFileArgs args, MethodInvoker action)
        {
            try
            {
                bool flag = GitCheckout.CheckoutBranch("Branch");
                if (flag)
                {
                    return new GitExecuteResult() { IsSuccess = true };
                }
                else
                {
                    return new GitExecuteResult() { IsSuccess = false, Message = "" };
                }
            }
            catch(Exception ex)
            {
                return new GitExecuteResult() { IsSuccess = false, Exp = ex };
            }
        }

        /// <summary>
        /// 克隆项目，将远程文件下载到本地，并创建源码关联
        /// </summary>
        /// <param name="args"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public GitExecuteResult Clone(GitFileArgs args, MethodInvoker action)
        {
            try
            {
                bool flag = GitClone.Clone(args.From.AbsolutePath, args.RootPath);
                if (flag)
                {
                    return new GitExecuteResult() { IsSuccess = true };
                }
                else
                {
                    return new GitExecuteResult() { IsSuccess = false, Message = "" };
                }
            }
            catch (Exception ex)
            {
                return new GitExecuteResult() { IsSuccess = false, Exp = ex };
            }
        }

        /// <summary>
        /// 提交代码修改（单个文件）
        /// </summary>
        /// <param name="args"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public GitExecuteResult Commit(GitFileArgs args, MethodInvoker action)
        {
            try
            {
                new GitCommit().Commit("");
                return new GitExecuteResult() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new GitExecuteResult() { IsSuccess = false, Exp = ex };
            }
        }

        /// <summary>
        /// 提交全部修改
        /// </summary>
        /// <param name="args"></param>
        /// <param name="action"></param>
        /// <returns></returns>
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
            try
            {
                string rootedPath = Repository.Init(args.RootPath);
                return new GitExecuteResult() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new GitExecuteResult() { IsSuccess = false, Exp = ex };
            }
            
        }

        public GitExecuteResult Log(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Merge(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Revert(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Ignore(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Unignore(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult Remove(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        public GitExecuteResult AddSloutionToSubversion(GitFileArgs args, MethodInvoker action)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
