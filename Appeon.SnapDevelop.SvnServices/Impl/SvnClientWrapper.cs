using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpSvn;
using SharpSvn.Security;
using SharpSvn.Remote;
using System.Windows.Forms;

namespace Appeon.SnapDevelop.SvnServices
{
    internal sealed class SvnClientWrapper : ISvnCommand, IDisposable
    {
        public bool IsAuthorizationEnabled { get; set; }
        public bool IsSharpSvnUIBinding { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string RemoteUri { get; set; }
        public string LocalUri { get; set; }

        Dictionary<string, Status> statusCache = new Dictionary<string, Status>(StringComparer.OrdinalIgnoreCase);
        SvnClient client;
        bool cancel;
        

        public SvnClientWrapper():this(null,null,null,null)
        {

        }
        public SvnClientWrapper(string user, string pwd) :this( null,null, user, pwd )
        {
        }
        public SvnClientWrapper(string remoteUri, string localUri, string user, string pwd)
        {
            UserName = user;
            PassWord = pwd;
            RemoteUri = remoteUri;
            LocalUri = localUri;

            CreateSvnClient();
        }
            

        public void CreateSvnClient()
        {
            if (client == null)
            {
                client = new SvnClient();

                client.Notify += client_Notify;
                client.Cancel += client_Cancel;

                if (!IsSharpSvnUIBinding)
                {
                    client.Authentication.Clear();
                    client.Authentication.UserNamePasswordHandlers += Authentication_UserNamePasswordHandlers;
                    client.Authentication.SslServerTrustHandlers += Authentication_SslServerTrustHandlers;
                }
            }
        }

        private void Authentication_SslServerTrustHandlers(object sender, SharpSvn.Security.SvnSslServerTrustEventArgs e)
        {
            e.AcceptedFailures = e.Failures;
            e.Save = true;
        }

        private void Authentication_UserNamePasswordHandlers(object sender, SharpSvn.Security.SvnUserNamePasswordEventArgs e)
        {
            e.UserName = UserName;
            e.Password = PassWord;
        }

        private void CheckAuthentication()
        {
            if (!IsAuthorizationEnabled) throw new SvnException("Failed to authorize.");
        }

        private void BindingSharpSvnUI( SvnClient client )
        {
            SharpSvn.UI.SvnUIBindArgs uIBindArgs = new SharpSvn.UI.SvnUIBindArgs();

            SharpSvn.UI.SvnUI.Bind(client, uIBindArgs);
            //It need to reference to winform assembly 
        }
        
        private void RemoveAllSvnAuthentication()
        {
          
            foreach( var svnAuthenCacheItem in
                client.Authentication.GetCachedItems( SvnAuthenticationCacheType.UserNamePassword))
            {
                svnAuthenCacheItem.Delete();
            }
            
        }


        #region Sharpsvn get functions

        public long GetLatestRevsion(string url)
        {

            SvnInfoEventArgs svnInfo;
            if (client.GetInfo(new SvnUriTarget(url), out svnInfo))
            {
                return svnInfo.LastChangeRevision;
            }

            return 0;
        }
        public string GetRepositoryRoot( string url )
        {
            var uri = new Uri(url);

            SvnRemoteSession remoteSession = new SvnRemoteSession(uri);
            Uri rootUrl;
            remoteSession.GetRepositoryRoot(out rootUrl);
            return rootUrl == null ? "" : rootUrl.AbsoluteUri;
        }
           

        #endregion

        #region Cancel support
        
        public void Cancel()
        {
            cancel = true;
        }

        void client_Cancel(object sender, SvnCancelEventArgs e)
        {
            e.Cancel = cancel;
        }
        #endregion

        #region Debug && event
        [System.Diagnostics.ConditionalAttribute("DEBUG")]
        static void Debug(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }

        void CheckNotDisposed()
        {
            if (client == null)
                throw new ObjectDisposedException("SvnClientWrapper");
        }

        void BeforeWriteOperation(string operationName)
        {
            BeforeReadOperation(operationName);
            ClearStatusCache();
        }

        void BeforeReadOperation(string operationName)
        {
            // before any subversion operation, ensure the object is not disposed
            // and register authorization if necessary
            CheckNotDisposed();
            
            cancel = false;
            if (OperationStarted != null)
                OperationStarted(this, new SubversionOperationEventArgs { Operation = operationName });
        }

        void AfterOperation()
        {
            // after any subversion operation, clear the memory pool
            if (OperationFinished != null)
                OperationFinished(this, EventArgs.Empty);
        }
        public void ClearStatusCache()
        {
            CheckNotDisposed();
            statusCache.Clear();
        }
        #endregion


        #region Notifications
        public event EventHandler<SubversionOperationEventArgs> OperationStarted;
        public event EventHandler OperationFinished;
        public event EventHandler<NotificationEventArgs> Notify;

        void client_Notify(object sender, SvnNotifyEventArgs e)
        {
            if (Notify != null)
            {
                Notify(this, new NotificationEventArgs()
                {
                    Action = GetActionString(e.Action),
                    Kind = GetKindString(e.NodeKind),
                    Path = e.Path
                });
            }
        }
        #endregion

        #region status->string conversion
        static string GetKindString(SvnNodeKind kind)
        {
            switch (kind)
            {
                case SvnNodeKind.Directory:
                    return "directory ";
                case SvnNodeKind.File:
                    return "file ";
                default:
                    return null;
            }
        }

        public static string GetActionString(SvnChangeAction action)
        {
            switch (action)
            {
                case SvnChangeAction.Add:
                    return GetActionString(SvnNotifyAction.CommitAdded);
                case SvnChangeAction.Delete:
                    return GetActionString(SvnNotifyAction.CommitDeleted);
                case SvnChangeAction.Modify:
                    return GetActionString(SvnNotifyAction.CommitModified);
                case SvnChangeAction.Replace:
                    return GetActionString(SvnNotifyAction.CommitReplaced);
                default:
                    return "unknown";
            }
        }

        static string GetActionString(SvnNotifyAction action)
        {
            switch (action)
            {
                case SvnNotifyAction.Add:
                case SvnNotifyAction.CommitAdded:
                    return "added";
                case SvnNotifyAction.Copy:
                    return "copied";
                case SvnNotifyAction.Delete:
                case SvnNotifyAction.UpdateDelete:
                case SvnNotifyAction.CommitDeleted:
                    return "deleted";
                case SvnNotifyAction.Restore:
                    return "restored";
                case SvnNotifyAction.Revert:
                    return "reverted";
                case SvnNotifyAction.RevertFailed:
                    return "revert failed";
                case SvnNotifyAction.Resolved:
                    return "resolved";
                case SvnNotifyAction.Skip:
                    return "skipped";
                case SvnNotifyAction.UpdateUpdate:
                    return "updated";
                case SvnNotifyAction.UpdateExternal:
                    return "updated external";
                case SvnNotifyAction.CommitModified:
                    return "modified";
                case SvnNotifyAction.CommitReplaced:
                    return "replaced";
                case SvnNotifyAction.LockFailedLock:
                    return "lock failed";
                case SvnNotifyAction.LockFailedUnlock:
                    return "unlock failed";
                case SvnNotifyAction.LockLocked:
                    return "locked";
                case SvnNotifyAction.LockUnlocked:
                    return "unlocked";
                default:
                    return "unknown";
            }
        }
        #endregion


        #region solution file execute
        static StatusKind ToStatusKind(SvnStatus kind)
        {
            switch (kind)
            {
                case SvnStatus.Added:
                    return StatusKind.Added;
                case SvnStatus.Conflicted:
                    return StatusKind.Conflicted;
                case SvnStatus.Deleted:
                    return StatusKind.Deleted;
                case SvnStatus.External:
                    return StatusKind.External;
                case SvnStatus.Ignored:
                    return StatusKind.Ignored;
                case SvnStatus.Incomplete:
                    return StatusKind.Incomplete;
                case SvnStatus.Merged:
                    return StatusKind.Merged;
                case SvnStatus.Missing:
                    return StatusKind.Missing;
                case SvnStatus.Modified:
                    return StatusKind.Modified;
                case SvnStatus.Normal:
                    return StatusKind.Normal;
                case SvnStatus.NotVersioned:
                    return StatusKind.Unversioned;
                case SvnStatus.Obstructed:
                    return StatusKind.Obstructed;
                case SvnStatus.Replaced:
                    return StatusKind.Replaced;
                default:
                    return StatusKind.None;
            }
        }

        public Status SingleStatus(string filename)
        {
            filename = FileUtility.NormalizePath(filename);
            Status result = null;
            if (statusCache.TryGetValue(filename, out result))
            {
                Debug("SVN: SingleStatus(" + filename + ") = cached " + result.TextStatus);
                return result;
            }
            Debug("SVN: SingleStatus(" + filename + ")");
            BeforeReadOperation("stat");
            try
            {
                SvnStatusArgs args = new SvnStatusArgs
                {
                    Revision = SvnRevision.Working,
                    RetrieveAllEntries = true,
                    RetrieveIgnoredEntries = true,
                    Depth = SvnDepth.Empty
                };
                client.Status(
                    filename, args,
                    delegate (object sender, SvnStatusEventArgs e) {
                        Debug("SVN: SingleStatus.callback(" + e.FullPath + "," + e.LocalContentStatus + ")");
                        System.Diagnostics.Debug.Assert(filename.ToString().Equals(e.FullPath, StringComparison.OrdinalIgnoreCase));
                        result = new Status
                        {
                            Copied = e.LocalCopied,
                            TextStatus = ToStatusKind(e.LocalContentStatus)
                        };
                    }
                );
                if (result == null)
                {
                    result = new Status
                    {
                        TextStatus = StatusKind.None
                    };
                }
                statusCache.Add(filename, result);
                return result;
            }
            catch (SvnException ex)
            {
                switch (ex.SvnErrorCode)
                {
                    case SvnErrorCode.SVN_ERR_WC_UPGRADE_REQUIRED:
                        result = new Status { TextStatus = StatusKind.None };
                        break;
                    case SvnErrorCode.SVN_ERR_WC_NOT_WORKING_COPY:
                        result = new Status { TextStatus = StatusKind.Unversioned };
                        break;
                    default:
                        throw new SvnClientException(ex);
                }
                statusCache.Add(filename, result);
                return result;
            }
            finally
            {
                AfterOperation();
            }
        }

        static SvnDepth ConvertDepth(Recurse recurse)
        {
            if (recurse == Recurse.Full)
                return SvnDepth.Infinity;
            else
                return SvnDepth.Empty;
        }

        public void Add(string filename, Recurse recurse)
        {
            Debug("SVN: Add(" + filename + ", " + recurse + ")");
            BeforeWriteOperation("add");
            try
            {
                client.Add(filename, ConvertDepth(recurse));
            }
            catch (SvnException ex)
            {
                throw new SvnClientException(ex);
            }
            finally
            {
                AfterOperation();
            }
        }

        public string GetPropertyValue(string fileName, string propertyName)
        {
            Debug("SVN: GetPropertyValue(" + fileName + ", " + propertyName + ")");
            BeforeReadOperation("propget");
            try
            {
                string propertyValue;
                if (client.GetProperty(fileName, propertyName, out propertyValue))
                    return propertyValue;
                else
                    return null;
            }
            catch (SvnException ex)
            {
                throw new SvnClientException(ex);
            }
            finally
            {
                AfterOperation();
            }
        }

        public void SetPropertyValue(string fileName, string propertyName, string newPropertyValue)
        {
            Debug("SVN: SetPropertyValue(" + fileName + ", " + propertyName + ", " + newPropertyValue + ")");
            BeforeWriteOperation("propset");
            try
            {
                if (newPropertyValue != null)
                    client.SetProperty(fileName, propertyName, newPropertyValue);
                else
                    client.DeleteProperty(fileName, propertyName);
            }
            catch (SvnException ex)
            {
                throw new SvnClientException(ex);
            }
            finally
            {
                AfterOperation();
            }
        }

        public void Delete(string[] files, bool force)
        {
            Debug("SVN: Delete(" + string.Join(",", files) + ", " + force + ")");
            BeforeWriteOperation("delete");
            try
            {
                client.Delete(
                    files,
                    new SvnDeleteArgs
                    {
                        Force = force
                    });
            }
            catch (SvnException ex)
            {
                throw new SvnClientException(ex);
            }
            finally
            {
                AfterOperation();
            }
        }

        public void Revert(string[] files, Recurse recurse)
        {
            Debug("SVN: Revert(" + string.Join(",", files) + ", " + recurse + ")");
            BeforeWriteOperation("revert");
            try
            {
                client.Revert(
                    files,
                    new SvnRevertArgs
                    {
                        Depth = ConvertDepth(recurse)
                    });
            }
            catch (SvnException ex)
            {
                throw new SvnClientException(ex);
            }
            finally
            {
                AfterOperation();
            }
        }

        public void Move(string from, string to, bool force)
        {
            Debug("SVN: Move(" + from + ", " + to + ", " + force + ")");
            BeforeWriteOperation("move");
            try
            {
                client.Move(
                    from, to,
                    new SvnMoveArgs
                    {
                        Force = force
                    });
            }
            catch (SvnException ex)
            {
                throw new SvnClientException(ex);
            }
            finally
            {
                AfterOperation();
            }
        }

        public void Copy(string from, string to)
        {
            Debug("SVN: Copy(" + from + ", " + to);
            BeforeWriteOperation("copy");
            try
            {
                client.Copy(from, to);
            }
            catch (SvnException ex)
            {
                throw new SvnClientException(ex);
            }
            finally
            {
                AfterOperation();
            }
        }

        public void AddToIgnoreList(string directory, params string[] filesToIgnore)
        {
            Debug("SVN: AddToIgnoreList(" + directory + ", " + string.Join(",", filesToIgnore) + ")");
            string propertyValue = GetPropertyValue(directory, "svn:ignore");
            StringBuilder b = new StringBuilder();
            if (propertyValue != null)
            {
                using (StringReader r = new StringReader(propertyValue))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {
                            b.AppendLine(line);
                        }
                    }
                }
            }
            foreach (string file in filesToIgnore)
                b.AppendLine(file);
            SetPropertyValue(directory, "svn:ignore", b.ToString());
        }

        public void Log(string[] paths, Revision start, Revision end,
                        int limit, bool discoverChangePaths, bool strictNodeHistory,
                        Action<LogMessage> logMessageReceiver)
        {
            Debug("SVN: Log({" + string.Join(",", paths) + "}, " + start + ", " + end +
                  ", " + limit + ", " + discoverChangePaths + ", " + strictNodeHistory + ")");
            BeforeReadOperation("log");
            try
            {
                client.Log(
                    paths,
                    new SvnLogArgs
                    {
                        Start = start,
                        End = end,
                        Limit = limit,
                        RetrieveChangedPaths = discoverChangePaths,
                        StrictNodeHistory = strictNodeHistory
                    },
                    delegate (object sender, SvnLogEventArgs e) {
                        try
                        {
                            Debug("SVN: Log: Got revision " + e.Revision);
                            LogMessage msg = new LogMessage()
                            {
                                Revision = e.Revision,
                                Author = e.Author,
                                Date = e.Time,
                                Message = e.LogMessage
                            };
                            if (discoverChangePaths)
                            {
                                msg.ChangedPaths = new List<ChangedPath>();
                                foreach (var entry in e.ChangedPaths)
                                {
                                    msg.ChangedPaths.Add(new ChangedPath
                                    {
                                        Path = entry.Path,
                                        CopyFromPath = entry.CopyFromPath,
                                        CopyFromRevision = entry.CopyFromRevision,
                                        Action = entry.Action
                                    });
                                }
                            }
                            logMessageReceiver(msg);
                        }
                        catch (Exception ex)
                        {
                            //MessageService.ShowException(ex);
                            Debug(ex.Message);
                        }
                    }
                );
                Debug("SVN: Log finished");
            }
            catch (SvnOperationCanceledException)
            {
                // allow cancel without exception
            }
            catch (SvnException ex)
            {
                throw new SvnClientException(ex);
            }
            finally
            {
                AfterOperation();
            }
        }

        public Stream OpenBaseVersion(string fileName)
        {
            MemoryStream stream = new MemoryStream();
            if (!this.client.Write(fileName, stream, new SvnWriteArgs() { Revision = SvnRevision.Base, ThrowOnError = false }))
                return null;
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public Stream OpenCurrentVersion(string fileName)
        {
            MemoryStream stream = new MemoryStream();
            if (!this.client.Write(fileName, stream, new SvnWriteArgs() { Revision = SvnRevision.Working }))
                return null;
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
        #endregion


        #region Command Operation

        public SvnExecuteResult Update( SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Update(args, action);
            }
//            var file = args.FileName;
//            Debug("SVN: update(" + file + ")");
//            BeforeWriteOperation("update");
//            try
//            {
//                SvnUpdateArgs updateArgs = new SvnUpdateArgs();
//                updateArgs.Depth = SvnDepth.Empty
//;
//                SvnUpdateResult updateResult;
//                var rel = client.Update(file, updateArgs, out updateResult);
//            }
//            catch (SvnException ex)
//            {
//                throw new SvnClientException(ex);
//            }
//            finally
//            {
//                AfterOperation();
//            }
        }
        public SvnExecuteResult Relocate(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Relocate(args, action);
            }
            //var file = args.FileName;
            //Debug("SVN: relocate(" + file + ")");
            //BeforeWriteOperation("relocate");
            //try
            //{
            //    SvnRelocateArgs relargs = new SvnRelocateArgs();
            //    var from = args.From;
            //    var to = args.To;
            //    var rel = client.Relocate(file, from, to, relargs);
            //}
            //catch (SvnException ex)
            //{
            //    throw new SvnClientException(ex);
            //}
            //finally
            //{
            //    AfterOperation();
            //}
        }
        public SvnExecuteResult Merge(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Merge(args, action);
            }
            //var file = args.FileName;
            //Debug("SVN: Merge(" + file + ")");
            //BeforeWriteOperation("merge");
            //try
            //{
            //    var path = args.From;
            //    SvnTarget svnTarget = SvnTarget.FromUri(path);

            //    var start = args.VersionStart;
            //    var end = args.VersionEnd;
            //    SvnRevisionRange revisionRange = new SvnRevisionRange(start, end);

            //    SvnMergeArgs mergeArgs = new SvnMergeArgs();
            //    mergeArgs.Depth = SvnDepth.Empty;

            //    var rel = client.Merge(file, svnTarget, revisionRange, mergeArgs);
            //}
            //catch (SvnException ex)
            //{
            //    throw new SvnClientException(ex);
            //}
            //finally
            //{
            //    AfterOperation();
            //}
        }
        public SvnExecuteResult Switch(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Switch(args, action);
            }
            //            var file = args.FileName;
            //            Debug("SVN: Switch(" + file + ")");
            //            BeforeWriteOperation("switch");
            //            try
            //            {
            //                SvnUriTarget uriTarget = new SvnUriTarget(args.From);
            //;
            //                SvnUpdateResult updateResult;
            //                var rel = client.Switch(file, uriTarget, out updateResult);
            //            }
            //            catch (SvnException ex)
            //            {
            //                throw new SvnClientException(ex);
            //            }
            //            finally
            //            {
            //                AfterOperation();
            //            }
        }
        public SvnExecuteResult Blame(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Blame(args, action);
            }
            //var file = args.FileName;
            //Debug("SVN: Blame(" + file + ")");
            //BeforeWriteOperation("blame");
            //try
            //{
            //    SvnTarget svnTarget = SvnTarget.FromUri( args.From );


            //    SvnBlameArgs blameArgs = new SvnBlameArgs();
            //    if( args.VersionStart > 0 ) blameArgs.Start = args.VersionStart;
            //    if (args.VersionEnd > 0) blameArgs.End = args.VersionEnd;

            //    var rel = client.Blame(svnTarget, blameArgs, (o,e)=> {
            //        LogMessage msg = new LogMessage()
            //        {
            //            Revision = e.Revision,
            //            Author = e.Author,
            //            Date = e.Time                        
            //        };

            //    });
            //}
            //catch (SvnException ex)
            //{
            //    throw new SvnClientException(ex);
            //}
            //finally
            //{
            //    AfterOperation();
            //}
        }
        public SvnExecuteResult Lock(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Lock(args, action);
            }
            //var file = args.FileName;
            //Debug("SVN: Lock(" + file + ")");
            //BeforeWriteOperation("lock");
            //try
            //{
            //    SvnLockArgs lockArgs = new SvnLockArgs();
            //    lockArgs.Comment = args.Comment;

            //    var rel = client.Lock(file, lockArgs);
            //}
            //catch (SvnException ex)
            //{
            //    throw new SvnClientException(ex);
            //}
            //finally
            //{
            //    AfterOperation();
            //}
        }
        public SvnExecuteResult Branch(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Branch(args, action);
            }

            //var file = args.FileName;
            //Debug("SVN: Branch(" + file + ")");
            //BeforeWriteOperation("branch");
            //try
            //{
            //    SvnTarget target = SvnTarget.FromString(file);
            //    SvnCommitResult  commitResult;
            //    SvnCopyArgs copyArgs = new SvnCopyArgs();


            //    var rel = client.RemoteCopy(target, args.To, copyArgs,out commitResult );
            //}
            //catch (SvnException ex)
            //{
            //    throw new SvnClientException(ex);
            //}
            //finally
            //{
            //    AfterOperation();
            //}
        }

        public SvnExecuteResult ShowSvnHelp()
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.ShowSvnHelp();
            }
        }

        public  SvnExecuteResult ShowSvnSettings()
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.ShowSvnSettings();
            }
        }

        public  SvnExecuteResult ShowSvnAbout()
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.ShowSvnAbout();
            }
        }

        public  SvnExecuteResult Diff(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Diff(args, action);
            }
            //var file = args.FileName;
            //Debug("SVN: Diff(" + file + ")");
            //BeforeWriteOperation("diff");
            //try
            //{
            //    SvnTarget svnTarget = SvnTarget.FromString(file);

            //    SvnRevisionRange revisionRange = new SvnRevisionRange(args.VersionStart, args.VersionEnd);

            //    SvnDiffArgs diffArgs = new SvnDiffArgs();
            //    diffArgs.Depth = SvnDepth.Empty;

            //    MemoryStream ms=new MemoryStream();

            //    var rel = client.Diff(svnTarget, revisionRange, diffArgs, ms);
            //}
            //catch (SvnException ex)
            //{
            //    throw new SvnClientException(ex);
            //}
            //finally
            //{
            //    AfterOperation();
            //}
        }

        public  SvnExecuteResult ConflictEditor(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.ConflictEditor(args, action);
            }
        }

        public  SvnExecuteResult ResolveConflict(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.ResolveConflict(args, action);
            }
        }

        public  SvnExecuteResult ShowLog(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.ShowLog(args, action);
            }
        }

        public  SvnExecuteResult Cleanup(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Cleanup(args, action);
            }
        }

        public  SvnExecuteResult RevisionGraph(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.RevisionGraph(args, action);
            }
        }

        public  SvnExecuteResult RepoStatus(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.RepoStatus(args, action);
            }
        }

        public  SvnExecuteResult RepoBrowser(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.RepoBrowser(args, action);
            }
        }

        public  SvnExecuteResult UpdateToRevision(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.UpdateToRevision(args, action);
            }
        }

        public  SvnExecuteResult Export(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Export(args, action);
            }
        }

        public  SvnExecuteResult ShowCheckoutDialog(MethodInvoker callback)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.ShowCheckoutDialog(callback);
            }
        }

        public  SvnExecuteResult ShowExportDialog(MethodInvoker callback)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.ShowExportDialog(callback);
            }
        }
      
        public  SvnExecuteResult ApplyPatch(SvnFileArgs args, MethodInvoker action)
        {
            //Proc("applypatch", fileName, callback);
            // TODO: Applying patches is not implemented.
            //MessageService.ShowMessage("Applying patches is not implemented.");
            return default(SvnExecuteResult);
        }

        public  SvnExecuteResult CreatePatch(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.CreatePatch(args, action);
            }
        }

        public  SvnExecuteResult Revert(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Revert(args, action);
            }

        }

        public  SvnExecuteResult Commit(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Commit(args, action);
            }
        }

        public  SvnExecuteResult Add(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
                return svnGui.Add(args, action);
            }
        }

        public  SvnExecuteResult Ignore(SvnFileArgs args, MethodInvoker action)
        {
            using (var svnGui = new SvnExternalWrapper())
            {
               return svnGui.Ignore(args, action);
            }
        }

        #endregion

        public void Dispose()
        {
            if (client != null)
                client.Dispose();
            client = null;
        }

       
    }






}
