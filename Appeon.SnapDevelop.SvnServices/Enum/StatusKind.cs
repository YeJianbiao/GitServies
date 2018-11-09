using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    public enum StatusKind
    {
        None,
        Added,
        Conflicted,
        Deleted,
        Modified,
        Replaced,
        External,
        Ignored,
        Incomplete,
        Merged,
        Missing,
        Obstructed,
        Normal,
        Unversioned
    }
}
