using Common.Enums;

namespace Common.Exceptions
{
    public class BusinessRuleException : ApplicationException
    {
        private const FunctionCode DefaultCode = FunctionCode.BusinessRuleViolation;

        public BusinessRuleException(string message)
            : base(DefaultCode, message) { }

        public BusinessRuleException(FunctionCode errorCode, string message)
            : base(errorCode, message) { }
    }
}
