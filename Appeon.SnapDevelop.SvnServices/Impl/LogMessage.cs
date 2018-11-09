using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    public class LogMessage
    {
        public long Revision;
        public string Author;
        public DateTime Date;
        public string Message;

        public List<ChangedPath> ChangedPaths;
    }
}
