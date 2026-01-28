using Common.Enums;

namespace Common.Exceptions
{
    public class ForbiddenException : ApplicationException
    {
        private const FunctionCode DefaultCode = FunctionCode.AccessDenied;

        // Alapértelmezett konstruktor
        public ForbiddenException(string message)
            : base(message, DefaultCode) { }

        public ForbiddenException(FunctionCode errorCode, string message)
            : base(errorCode, message) { }

        public static ForbiddenException MissingPermission(string permission)
        {
            return new ForbiddenException(
                FunctionCode.MissingPermission,
                $"Permission required for this operation is missing: {permission}");
        }
    }
}
