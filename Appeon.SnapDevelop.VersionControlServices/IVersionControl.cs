using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.VersionControlServices
{
    public interface IVersionControl
    {
        ExecuteResult FileCreated( FileArgs arg);

        ExecuteResult FileRemoving( FileCancelArgs arg);

        ExecuteResult FileCopying( FileRenamingArgs arg);

        ExecuteResult FileRenaming( FileRenamingArgs arg);

        ExecuteResult FileSaved( FileArgs arg);

        void VersionControlCommand( CommandArgs arg);
    }




}
