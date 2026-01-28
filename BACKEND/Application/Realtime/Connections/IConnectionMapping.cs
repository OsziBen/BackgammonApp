namespace Application.Realtime.Connections
{
    public interface IConnectionMapping
    {
        void Add(string connectionId, Guid gamePlayerId);
        bool TryGet(string connectionId, out Guid gamePlayerId);
        void Remove(string connectionId);
    }
}
