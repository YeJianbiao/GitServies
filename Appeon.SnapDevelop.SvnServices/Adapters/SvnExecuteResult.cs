using System;
using Appeon.SnapDevelop.VersionControlServices;


namespace Appeon.SnapDevelop.SvnServices
{
    public class SvnExecuteResult :ExecuteResult
    {
        //errorcode  1 do not contain root path.
        //2 is not exists in version control.
        //3 cancel is true;
        //
        //-1 svn client catch error
        //-2 svn client knownerror
        //-3 exception error.

    }
}
