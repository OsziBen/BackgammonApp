using Application.Realtime.Connections;
using System.Collections.Concurrent;

namespace Infrastructure.Realtime.Connections
{
    public class InMemoryConnectionMapping : IConnectionMapping
    {
        private readonly ConcurrentDictionary<string, Guid> _map = new();

        public void Add(string connectionId, Guid gamePlayerId)
            => _map[connectionId] = gamePlayerId;

        public bool TryGet(string connectionId, out Guid gamePlayerId)
            => _map.TryGetValue(connectionId, out gamePlayerId);

        public void Remove(string connectionId)
            => _map.TryRemove(connectionId, out _);
    }
}
