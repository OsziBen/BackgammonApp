using Application.GameSessions.Responses;
using Domain.GameSession;

namespace Application.GameSessions.Services.GameSessionSnapshotFactory
{
    public interface IGameSessionSnapshotFactory
    {
        GameSessionSnapshotResponse Create(
            GameSession session,
            Guid? localPlayerId = null,
            bool? isRejoin = false);
    }
}
