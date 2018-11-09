using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;


namespace Appeon.SnapDevelop.SvnServices
{
    internal sealed class SvnExternalWrapper: ISvnCommand, IDisposable
    {
        static string GetPathFromRegistry(RegistryHive hive, string valueName)
        {
            RegistryView view = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Default;
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, view))
            {
                using (RegistryKey key = baseKey.OpenSubKey("SOFTWARE\\TortoiseSVN"))
                {
                    if (key != null)
                        return key.GetValue(valueName) as string;
                    else
                        return null;
                }
            }
        }

        static string GetPathFromRegistry(string valueName)
        {
            return GetPathFromRegistry(RegistryHive.CurrentUser, valueName)
                ?? GetPathFromRegistry(RegistryHive.LocalMachine, valueName);
        }

        static SvnExecuteResult Proc(string command, string fileName, MethodInvoker callback)
        {
           return Proc(command, fileName, callback, null);
        }

        static SvnExecuteResult Proc(string command, string fileName, MethodInvoker callback, string argument)
        {
            SvnExecuteResult result = new SvnExecuteResult() { ErrorCode = 0 };

            string path = GetPathFromRegistry("ProcPath");
            if (path == null)
            {
                result.ErrorCode = -1;
                result.Message = "Install:TortoiseSVN is required, Please download from http://tortoisesvn.net/";
            }
            else
            {
                try
                {
                    StringBuilder arguments = new StringBuilder();
                    arguments.Append("/command:");
                    arguments.Append(command);
                    if (fileName != null)
                    {
                        arguments.Append(" /notempfile ");
                        arguments.Append(" /path:\"");
                        arguments.Append(fileName);
                        arguments.Append('"');
                    }
                    if (argument != null)
                    {
                        arguments.Append(' ');
                        arguments.Append(argument);
                    }
                    Process p = new Process();
                    p.StartInfo.FileName = path;
                    p.StartInfo.Arguments = arguments.ToString();
                    //p.StartInfo.RedirectStandardError = true;
                    //p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.UseShellExecute = false;
                    p.EnableRaisingEvents = true;
                    p.Exited += delegate {
                        p.Dispose();
                        if (callback != null) { callback(); }
                    };
                   
                    p.Start();
                }
                catch (Exception ex)
                {
                    result.ErrorCode = -3;
                    result.Message = "Error:" + ex.Message;
                }
            }
            return result;
        }

        public SvnExecuteResult ShowCheckoutDialog(MethodInvoker callback)
        {
            return Proc("checkout", null, callback);
        }

        public SvnExecuteResult ShowExportDialog(MethodInvoker callback)
        {
            return Proc("export", null, callback);
        }

        public SvnExecuteResult Update( SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("update", fileName, callback);
        }

        public  SvnExecuteResult ApplyPatch(SvnFileArgs args, MethodInvoker callback)
        {
            //Proc("applypatch", fileName, callback);
            // TODO: Applying patches is not implemented.
            //MessageService.ShowMessage("Applying patches is not implemented.");
            return default(SvnExecuteResult);
        }

        public  SvnExecuteResult CreatePatch(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("createpatch", fileName, callback);
        }

        public  SvnExecuteResult Revert(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("revert", fileName, callback);
        }

        public  SvnExecuteResult Commit(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("commit", fileName, callback);
        }

        public  SvnExecuteResult Add(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("add", fileName, callback);
        }

        public  SvnExecuteResult Ignore(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("ignore", fileName, callback);
        }

        public SvnExecuteResult ShowSvnHelp()
        {
            return Proc("help", null, null);
        }

        public SvnExecuteResult ShowSvnSettings()
        {
            return Proc("settings", null, null);
        }

        public SvnExecuteResult ShowSvnAbout()
        {
            return Proc("about", null, null);
        }

        public SvnExecuteResult Diff(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("diff", fileName, callback);
        }

        public SvnExecuteResult ConflictEditor(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("conflicteditor", fileName, callback);
        }

        public  SvnExecuteResult ResolveConflict(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("resolve", fileName, callback);
        }

        public  SvnExecuteResult ShowLog(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("log", fileName, callback);
        }

        public SvnExecuteResult Cleanup(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("cleanup", fileName, callback);
        }

        public SvnExecuteResult RevisionGraph(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("revisiongraph", fileName, callback);
        }

        public SvnExecuteResult RepoStatus(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("repostatus", fileName, callback);
        }

        public SvnExecuteResult RepoBrowser(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("repobrowser", fileName, callback);
        }

        public SvnExecuteResult UpdateToRevision(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("update", fileName, callback, "/rev");
        }

        public SvnExecuteResult Export(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("export", fileName, callback);
        }

        public  SvnExecuteResult Branch(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("copy", fileName, callback);
        }

        public SvnExecuteResult Lock(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("lock", fileName, callback);
        }

        public SvnExecuteResult Blame(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("blame", fileName, callback);
        }

        public SvnExecuteResult Switch(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("switch", fileName, callback);
        }

        public SvnExecuteResult Merge(SvnFileArgs args, MethodInvoker callback)
        {
            string fileName = args.FileName;
            return Proc("merge", fileName, callback);
        }

        public SvnExecuteResult Relocate( SvnFileArgs args, MethodInvoker callback)
        {
            var fileName = args.FileName;
            return Proc("relocate", fileName, callback);
        }

        public void Dispose()
        {
             
        }
    }
}
