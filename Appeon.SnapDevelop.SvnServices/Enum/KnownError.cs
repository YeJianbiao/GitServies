using SharpSvn;

namespace Appeon.SnapDevelop.SvnServices
{
    public enum KnownError
    {
        FileNotFound = SvnErrorCode.SVN_ERR_FS_NOT_FOUND,
        CannotDeleteFileWithLocalModifications = SvnErrorCode.SVN_ERR_CLIENT_MODIFIED,
        CannotDeleteFileNotUnderVersionControl = SvnErrorCode.SVN_ERR_UNVERSIONED_RESOURCE
    }
}
