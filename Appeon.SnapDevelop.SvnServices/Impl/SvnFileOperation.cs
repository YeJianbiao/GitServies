using System;
using System.IO;
using System.Text;
using System.Windows.Forms;



namespace Appeon.SnapDevelop.SvnServices
{
    internal sealed class SvnFileOperation: IFileOperation
    {
        //errorcode  1 do not contain root path.
        //2 is not exists in version control.
        //3 cancel is true;
        //
        //-1 svn client catch error
        //-2 svn client knownerror
        //-3 exception error.
       
        void AddFileWithParentDirectoriesToSvn(SvnClientWrapper client, string fileName)
        {
            if (!CanBeVersionControlledFile(fileName))
            {
                AddFileWithParentDirectoriesToSvn(client, FileUtility.GetAbsolutePath(fileName, ".."));
            }
            Status status = client.SingleStatus(fileName);
            if (status.TextStatus != StatusKind.Unversioned)
                return;
            client.Add(fileName, Recurse.None);
        }

        #region Is it a svn file ?
        internal static bool CanBeVersionControlledFile(string fileName)
        {
            return CanBeVersionControlledDirectory(Path.GetDirectoryName(fileName));
        }

        internal static bool CanBeVersionControlledDirectory(string directory)
        {
            return FindWorkingCopyRoot(directory) != null;
        }
        #endregion

        static string FindWorkingCopyRoot(string directory)
        {         
            try
            {
                if (!Path.IsPathRooted(directory))
                    return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
            DirectoryInfo info = new DirectoryInfo(directory);
            while (info != null)
            {
                if (Directory.Exists(Path.Combine(info.FullName, ".svn")))
                    return info.FullName;
                info = info.Parent;
            }
            return null;
        }
                
        public SvnExecuteResult FileCreated( SvnFileArgs e)
        {
            SvnExecuteResult result = new SvnExecuteResult() { ErrorCode =0};

            if (!Path.IsPathRooted(e.FileName))
            {
                result.ErrorCode = 1;
                return result;
            }

            string fullName = Path.GetFullPath(e.FileName);
            if (!CanBeVersionControlledFile(fullName))
            {
                result.ErrorCode = 2;
                return result;
            }
            try
            {
                using (SvnClientWrapper client = new SvnClientWrapper())
                {
                    SvnMessageView.HandleNotifications(client);

                    Status status = client.SingleStatus(fullName);
                    switch (status.TextStatus)
                    {
                        case StatusKind.Unversioned:
                        case StatusKind.Deleted:
                            client.Add(fullName, Recurse.None);
                            break;
                    }
                }
            }
            catch (SvnClientException ex)
            {
                result.ErrorCode = -1;
                result.Message = ex.Message;
            }
            catch ( Exception ex )
            {
                result.ErrorCode = -3;
                result.Message = ex.Message;
            }
            return result;
        }

        public SvnExecuteResult FileRemoving( SvnFileCancelArgs e)
        {
            SvnExecuteResult result = new SvnExecuteResult() { ErrorCode = 0 };

            if (e.Cancel)
            {
                result.ErrorCode = 3;
                return result;
            };
            string fullName = Path.GetFullPath(e.FileName);
            if (!CanBeVersionControlledFile(fullName))
            {
                result.ErrorCode = 2;
                return result;
            }

            var forceDel = e.IsForce;

            if (e.IsDirectory)
            {
                // show "cannot delete directories" message even if
                // AutomaticallyDeleteFiles (see below) is off!
                using (SvnClientWrapper client = new SvnClientWrapper())
                {
                    SvnMessageView.HandleNotifications(client);

                    try
                    {
                        Status status = client.SingleStatus(fullName);
                        switch (status.TextStatus)
                        {
                            case StatusKind.None:
                            case StatusKind.Unversioned:
                            case StatusKind.Ignored:
                                break;
                            default:
                                // must be done using the subversion client, even if
                                // AutomaticallyDeleteFiles is off, because we don't want to corrupt the
                                // working copy
                                e.OperationAlreadyDone = true;
                                try
                                {
                                    client.Delete(new string[] { fullName }, forceDel);
                                }
                                catch (SvnClientException ex)
                                {
                                    if (ex.IsKnownError(KnownError.CannotDeleteFileWithLocalModifications)
                                        || ex.IsKnownError(KnownError.CannotDeleteFileNotUnderVersionControl))
                                    {

                                        try
                                        {
                                            client.Delete(new string[] { fullName }, true);
                                        }
                                        catch (SvnClientException ex2)
                                        {
                                            e.Cancel = true;
                                            result.ErrorCode = -2;
                                            result.Message = ex2.Message;
                                        }
                                       
                                    }
                                    else
                                    {
                                        e.Cancel = true;
                                        result.ErrorCode = -1;
                                        result.Message = ex.Message;
                                    }
                                }
                                break;
                        }
                    }
                    catch (SvnClientException ex3)
                    {
                        e.Cancel = true;
                        result.ErrorCode = -2;
                        result.Message = ex3.Message;
                    }
                    catch( Exception ex)
                    {
                        e.Cancel = true;
                        result.ErrorCode = -3;
                        result.Message = ex.Message;
                    }
                }
                return result;
            }
            // not a directory, but a file:
                        
            try
            {
                using (SvnClientWrapper client = new SvnClientWrapper())
                {
                    SvnMessageView.HandleNotifications(client);

                    Status status = client.SingleStatus(fullName);
                    switch (status.TextStatus)
                    {
                        case StatusKind.None:
                        case StatusKind.Unversioned:
                        case StatusKind.Ignored:
                        case StatusKind.Deleted:
                            return result; // nothing to do
                        case StatusKind.Normal:
                            // remove without problem
                            break;
                        case StatusKind.Modified:
                        case StatusKind.Replaced:
                            // modified files cannot be deleted, so we need to revert the changes first
                            if (forceDel)
                            {
                                client.Revert(new string[] { fullName }, e.IsDirectory ? Recurse.Full : Recurse.None);
                            }else
                            {
                                e.Cancel = true;
                                result.ErrorCode = 3;
                                result.Message = $"Subversion need to revert local modifications file({fullName}).";
                                return result;
                            }
                            break;
                        case StatusKind.Added:
                            if (status.Copied && !forceDel) // already copyed
                            {
                                result.ErrorCode = 3;
                                result.Message = $"({fullName})File is copied.";
                                e.Cancel = true;
                                return result;
                            }
                            client.Revert(new string[] { fullName }, e.IsDirectory ? Recurse.Full : Recurse.None);
                            return result;
                        default:
                            result.ErrorCode = 3;
                            result.Message = $"Subversion Cannot Remove file({fullName}),{status.TextStatus.ToString()}";
                            e.Cancel = true;
                            return result;
                    }
                    e.OperationAlreadyDone = true;
                    client.Delete(new string[] { fullName }, true);
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = -3;
                result.Message = "File removed exception: " + ex;
            }
            return result;
        }

        public SvnExecuteResult FileCopying( SvnFileRenamingArgs e)
        {
            SvnExecuteResult result = new SvnExecuteResult() { ErrorCode = 0 };
            if (e.Cancel)
            {
                result.ErrorCode = 3;
                return result;
            };

            string fullSource = Path.GetFullPath(e.SourceFile);
            if (!CanBeVersionControlledFile(fullSource))
            {
                result.ErrorCode = 2;
                return result;
            }
            string fullTarget = Path.GetFullPath(e.TargetFile);
            if (!CanBeVersionControlledFile(fullTarget))
            {
                result.ErrorCode = 2;
                return result;
            }
            try
            {
                using (SvnClientWrapper client = new SvnClientWrapper())
                {
                    SvnMessageView.HandleNotifications(client);

                    Status status = client.SingleStatus(fullSource);
                    switch (status.TextStatus)
                    {
                        case StatusKind.Unversioned:
                        case StatusKind.None:
                            return result; // nothing to do
                        case StatusKind.Normal:
                        case StatusKind.Modified:
                        case StatusKind.Replaced:
                        case StatusKind.Added:
                            // copy without problem
                            break;
                        default:
                            result.ErrorCode = 3;
                            result.Message = $"Subversion Cannot Copy file({fullSource}), {status.TextStatus.ToString()}";
                            e.Cancel = true;
                            return result;
                    }
                    e.OperationAlreadyDone = true;
                    client.Copy(fullSource, fullTarget);
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = -3;
                result.Message = "File renamed exception: " + ex;
            }
            return result;
        }

        bool CheckRenameOrReplacePossible(SvnFileRenamingArgs e, bool replaceAllowed = false)
        {
            if (e.IsDirectory && Directory.Exists(e.SourceFile))
            {
                if (!replaceAllowed && Directory.Exists(e.TargetFile))
                {
                     
                    return false;
                }
            }
            else if (File.Exists(e.SourceFile))
            {
                if (!replaceAllowed && File.Exists(e.TargetFile))
                {
                     
                    return false;
                }
            }
            return true;
        }

        public SvnExecuteResult FileRenaming( SvnFileRenamingArgs e)
        {
            SvnExecuteResult result = new SvnExecuteResult() { ErrorCode = 0 };
                
            if (e.Cancel)
            {
                result.ErrorCode = 3;
                return result;
            };

            string fullSource = Path.GetFullPath(e.SourceFile);
            if (!CanBeVersionControlledFile(fullSource))
            {
                result.ErrorCode = 2;
                return result;
            }
            if (!CheckRenameOrReplacePossible(e))
            {
                result.ErrorCode = 3;
                result.Message = $"({e.TargetFile })File is using.";
                e.Cancel = true;
                return result;
            }
            try
            {
                using (SvnClientWrapper client = new SvnClientWrapper())
                {
                    SvnMessageView.HandleNotifications(client);

                    Status status = client.SingleStatus(fullSource);
                    switch (status.TextStatus)
                    {
                        case StatusKind.Unversioned:
                        case StatusKind.None:
                        case StatusKind.Ignored:
                            return result; // nothing to do
                        case StatusKind.Normal:
                        case StatusKind.Modified:
                        case StatusKind.Replaced:
                        case StatusKind.Added:
                            // rename without problem
                            break;
                        default:
                            result.ErrorCode = 3;
                            result.Message = $"Subversion cannot rename file({fullSource}), {status.TextStatus.ToString()}";
                            e.Cancel = true;
                            return result;
                    }
                    e.OperationAlreadyDone = true;
                    client.Move(fullSource,
                                Path.GetFullPath(e.TargetFile),
                                true
                               );
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = -3;
                result.Message = "File renamed exception: " + ex;
            }
            return result;
        }

        public SvnExecuteResult FileSaved( SvnFileArgs arg)
        {
            SvnExecuteResult result = new SvnExecuteResult() { ErrorCode = 0 };
            string fileName = arg.FileName;
            if (!CanBeVersionControlledFile(fileName)) return result;


            return result;
        }

        
    }
}
