using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Appeon.SnapDevelop.SvnServices
{
    public interface ISvnCommand
    {
        SvnExecuteResult Add(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult ApplyPatch(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Blame(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Branch(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Cleanup(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Commit(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult ConflictEditor(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult CreatePatch(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Diff(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Export(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Ignore(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Lock(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Merge(SvnFileArgs args, MethodInvoker action = null);
        SvnExecuteResult Relocate(SvnFileArgs args, MethodInvoker action = null);
        SvnExecuteResult RepoBrowser(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult RepoStatus(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult ResolveConflict(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Revert(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult RevisionGraph(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult ShowCheckoutDialog(MethodInvoker callback);
        SvnExecuteResult ShowExportDialog(MethodInvoker callback);
        SvnExecuteResult ShowLog(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult ShowSvnAbout();
        SvnExecuteResult ShowSvnHelp();
        SvnExecuteResult ShowSvnSettings();
        SvnExecuteResult Switch(SvnFileArgs args, MethodInvoker action = null);
        SvnExecuteResult UpdateToRevision(SvnFileArgs args, MethodInvoker action);
        SvnExecuteResult Update(SvnFileArgs file, MethodInvoker action = null);



    }
}
