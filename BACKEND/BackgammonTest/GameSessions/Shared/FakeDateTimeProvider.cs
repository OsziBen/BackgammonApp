using Application.Shared.Time;

namespace BackgammonTest.GameSessions.Shared
{
    public class FakedateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow { get; private set; }

        public FakedateTimeProvider(DateTimeOffset initialTime)
        {
            UtcNow = initialTime;
        }

        public void Advance(TimeSpan by)
        {
            UtcNow = UtcNow.Add(by);
        }

        public void Set(DateTimeOffset value)
        {
            UtcNow = value;
        }
    }
}
