using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class Cache
    {

        public static IList<string> UncommitChanges = new List<string>();

        public static Queue<string> FileChanges = new Queue<string>();


    }
}
