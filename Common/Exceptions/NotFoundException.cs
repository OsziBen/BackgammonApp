using Common.Enums;

namespace Common.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        private const FunctionCode DefaultCode = FunctionCode.ResourceNotFound;

        public NotFoundException(string message)
            : base(message, DefaultCode) { }

        public NotFoundException(FunctionCode errorCode, string message)
            : base(errorCode, message) { }

        public static NotFoundException CreateForResource(string resourceName, object key)
        {
            return new NotFoundException($"Entity ({key}) of type '{resourceName}' not found.");
        }
    }
}
