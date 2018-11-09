using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    public interface IFileOperation
    {
        SvnExecuteResult FileCreated(SvnFileArgs e);
        SvnExecuteResult FileRemoving(SvnFileCancelArgs e);
        SvnExecuteResult FileCopying(SvnFileRenamingArgs e);
        SvnExecuteResult FileRenaming(SvnFileRenamingArgs e);

        SvnExecuteResult FileSaved(SvnFileArgs arg);
    }
}
