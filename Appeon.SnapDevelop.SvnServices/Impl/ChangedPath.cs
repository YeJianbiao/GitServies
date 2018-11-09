using System;
using SharpSvn;

namespace Appeon.SnapDevelop.SvnServices
{
    public class ChangedPath
    {
        public string Path;
        public string CopyFromPath;
        public long CopyFromRevision;
        /// <summary>
        /// change action ('A','D','R' or 'M')
        /// </summary>
        public SvnChangeAction Action;
    }
}
