﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.VersionControlServices
{    

    public abstract class FileCancelArgs : FileArgs
    {

        public  bool Cancel { get; set; }

        public  bool OperationAlreadyDone { get; set; }

        
      
    }
}
