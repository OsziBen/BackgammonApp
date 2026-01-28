using Common.Enums;

namespace Common.Exceptions
{
    public interface IApplicationException
    {
        FunctionCode ErrorCode { get; }
    }
}
