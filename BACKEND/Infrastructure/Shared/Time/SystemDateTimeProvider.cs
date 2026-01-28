using Application.Shared.Time;

namespace Infrastructure.Shared.Time
{
    public class SystemdateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
