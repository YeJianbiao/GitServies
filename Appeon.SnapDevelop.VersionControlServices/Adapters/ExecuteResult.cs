using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.VersionControlServices
{
    public abstract class ExecuteResult
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }

    }
}
