using Application.Interfaces;
using Domain.GamePlayer;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GamePlayerRepository : BaseRepository<GamePlayer>, IGamePlayerRepository
    {
        public GamePlayerRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public Task<GamePlayer?> GetBySessionAndUserAsync(
            Guid sessionId,
            Guid userId,
            bool asNoTracking = false)
            => Query(asNoTracking: asNoTracking)
                .FirstOrDefaultAsync(gp =>
                    gp.GameSessionId == sessionId &&
                    gp.UserId == userId);

        public Task<GamePlayer?> GetOpponentAsync(
            Guid sessionId,
            Guid excludePlayerId,
            bool asNoTracking = false)
            => Query(asNoTracking: asNoTracking)
                .FirstOrDefaultAsync(gp =>
                    gp.GameSessionId == sessionId &&
                    gp.UserId != excludePlayerId);

        public Task<List<GamePlayer>> GetPlayersBySessionAsync(
            Guid sessionId,
            bool asNoTracking = false)
            => Query(asNoTracking: asNoTracking)
                .Where(gp => gp.GameSessionId == sessionId)
                .ToListAsync();
    }
}
