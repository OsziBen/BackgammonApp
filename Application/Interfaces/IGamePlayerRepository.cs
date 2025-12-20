using Domain.GamePlayer;

namespace Application.Interfaces
{
    public interface IGamePlayerRepository : IReadRepository<GamePlayer>, IBaseRepository<GamePlayer>
    {
        Task<GamePlayer?> GetBySessionAndUserAsync(
            Guid sessionId,
            Guid userId,
            bool asNoTracking = false);

        Task<GamePlayer?> GetOpponentAsync(
            Guid sessionId,
            Guid excludePlayerId,
            bool asNoTracking = false);
    }
}
