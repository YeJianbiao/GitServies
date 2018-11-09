using System;
using Appeon.SnapDevelop.VersionControlServices;

namespace Appeon.SnapDevelop.SvnServices
{
    public sealed class SvnService : IVersionControl
    {
        IFileOperation FileOperation;

        public SvnService()
        {
            FileOperation = new SvnFileOperation();

            SubversionCommand.RegistCommand<AboutCommand>(CommandType.AboutCommand);
            SubversionCommand.RegistCommand<AddCommand>(CommandType.AddCommand);
            SubversionCommand.RegistCommand<ApplyPatchCommand>(CommandType.ApplyPatchCommand);
            SubversionCommand.RegistCommand<BlameCommand>(CommandType.BlameCommand);
            SubversionCommand.RegistCommand<BranchCommand>(CommandType.BranchCommand);
            SubversionCommand.RegistCommand<CleanupCommand>(CommandType.CleanupCommand);
            SubversionCommand.RegistCommand<CommitCommand>(CommandType.CommitCommand);
            SubversionCommand.RegistCommand<CreatePatchCommand>(CommandType.CreatePatchCommand);
            SubversionCommand.RegistCommand<DiffCommand>(CommandType.DiffCommand);
            SubversionCommand.RegistCommand<EditConflictsCommand>(CommandType.EditConflictsCommand);
            SubversionCommand.RegistCommand<ExportWorkingCopyCommand>(CommandType.ExportWorkingCopyCommand);
            SubversionCommand.RegistCommand<HelpCommand>(CommandType.HelpCommand);
            SubversionCommand.RegistCommand<IgnoreCommand>(CommandType.IgnoreCommand);
            SubversionCommand.RegistCommand<LockCommand>(CommandType.LockCommand);
            SubversionCommand.RegistCommand<MergeCommand>(CommandType.MergeCommand);
            SubversionCommand.RegistCommand<RelocateCommand>(CommandType.RelocateCommand);
            SubversionCommand.RegistCommand<RepoBrowserCommand>(CommandType.RepoBrowserCommand);
            SubversionCommand.RegistCommand<RepoStatusCommand>(CommandType.RepoStatusCommand);
            SubversionCommand.RegistCommand<ResolveConflictsCommand>(CommandType.ResolveConflictsCommand);
            SubversionCommand.RegistCommand<RevertCommand>(CommandType.RevertCommand);
            SubversionCommand.RegistCommand<RevisionGraphCommand>(CommandType.RevisionGraphCommand);
            SubversionCommand.RegistCommand<SettingsCommand>(CommandType.SettingsCommand);
            SubversionCommand.RegistCommand<ShowLogCommand>(CommandType.ShowLogCommand);
            SubversionCommand.RegistCommand<ShowCheckoutCommand>(CommandType.ShowCheckoutCommand);
            SubversionCommand.RegistCommand<ShowExportCommand>(CommandType.ShowExportCommand);
            SubversionCommand.RegistCommand<SwitchCommand>(CommandType.SwitchCommand);
            SubversionCommand.RegistCommand<UnignoreCommand>(CommandType.UnignoreCommand);
            SubversionCommand.RegistCommand<UpdateCommand>(CommandType.UpdateCommand);
            SubversionCommand.RegistCommand<UpdateToRevisionCommand>(CommandType.UpdateToRevisionCommand);
        }
                
        public ExecuteResult FileCopying(FileRenamingArgs arg)
        {
            return FileOperation.FileCopying((SvnFileRenamingArgs)arg);
        }

        public ExecuteResult FileCreated(FileArgs arg)
        {
            return FileOperation.FileCreated((SvnFileArgs)arg);
        }

        public ExecuteResult FileRemoving(FileCancelArgs arg)
        {
            return FileOperation.FileRemoving((SvnFileCancelArgs)arg);
        }

        public ExecuteResult FileRenaming(FileRenamingArgs arg)
        {
            return FileOperation.FileRenaming((SvnFileRenamingArgs)arg);
        }

        public ExecuteResult FileSaved(FileArgs arg)
        {
            return FileOperation.FileSaved((SvnFileArgs)arg);
        }

        public void VersionControlCommand(CommandArgs arg)
        {
            SubversionCommand.GetCommand(arg.CmdType).Run((SvnFileArgs)arg.Args);
        }

        public void Command_Callback()
        {

        }

    }



}
