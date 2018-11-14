

using Appeon.SnapDevelop.VersionControlServices;
using System;

namespace Appeon.SnapDevelop.GitServices
{
    public class GitExecuteResult : ExecuteResult
    {

        public bool IsSuccess { get; set; }

        public Exception Exp { get; set; }

    }
}
