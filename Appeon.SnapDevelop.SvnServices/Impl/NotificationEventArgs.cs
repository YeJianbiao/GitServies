using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    internal class NotificationEventArgs : EventArgs
    {
        public string Action;
        public string Kind;
        public string Path;
    }
}
