using Domain.GameSession;

namespace Application.Interfaces
{
    public interface IGameSessionRepository : IReadRepository<GameSession>, IBaseRepository<GameSession>
    {
        Task<GameSession?> GetBySessionCodeAsync(
            string sessionCode,
            bool includePlayers = true,
            bool asNoTracking = false);
    }
}
