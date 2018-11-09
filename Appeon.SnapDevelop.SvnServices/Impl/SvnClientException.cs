using System;
using SharpSvn;

namespace Appeon.SnapDevelop.SvnServices
{
    public class SvnClientException : Exception
    {
        SvnErrorCode errorCode;

        internal SvnClientException(SvnException ex) : base(ex.Message, ex)
        {
            this.errorCode = ex.SvnErrorCode;
            
        }

        /// <summary>
        /// Gets the inner exception of the exception being wrapped.
        /// </summary>
        public Exception GetInnerException()
        {
            return InnerException.InnerException;
        }

        public bool IsKnownError(KnownError knownError)
        {
            return (int)errorCode == (int)knownError;
        }
    }
}
