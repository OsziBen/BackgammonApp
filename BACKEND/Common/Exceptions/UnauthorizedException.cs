using Common.Enums;

namespace Common.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        private const FunctionCode DefaultCode = FunctionCode.Unauthorized;

        // Alapértelmezett konstruktor
        public UnauthorizedException(string message)
            : base(message, DefaultCode) { }

        public UnauthorizedException(FunctionCode errorCode, string message)
            : base(errorCode, message) { }

    }
}
