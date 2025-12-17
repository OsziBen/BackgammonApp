using Common.Enums;

namespace Common.Exceptions
{
    public class ValidationFailedException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationFailedException(
            IDictionary<string, string[]> errors,
            string message = "One or more validation errors occurred.")
            : base(FunctionCode.ValidationError, message)
        {
            Errors = errors;
        }
    }
}
