using Common.Enums;

namespace Common.Exceptions
{
    public class ApplicationException : Exception, IApplicationException
    {
        public FunctionCode ErrorCode { get; }

        protected ApplicationException(string message, FunctionCode defaultCode)
            : base(message)
        {
            this.ErrorCode = defaultCode;
        }

        protected ApplicationException(FunctionCode errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        protected ApplicationException(string message, FunctionCode defaultCode, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = defaultCode;
        }
    }
}
