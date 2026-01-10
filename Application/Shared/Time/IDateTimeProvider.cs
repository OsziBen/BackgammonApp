namespace Application.Shared.Time
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow { get; }
    }
}
