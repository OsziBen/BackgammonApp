using Application.Interfaces.Repository.GamePlayer;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GamePlayer
{
    public class GamePlayerReadRepository
        : ReadRepositoryBase<Domain.GamePlayer.GamePlayer>,
        IGamePlayerReadRepository
    {
        public GamePlayerReadRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public Task<Domain.GamePlayer.GamePlayer?> GetBySessionAndUserAsync(
            Guid sessionId,
            Guid userId)
            => Query()
                .FirstOrDefaultAsync(gp =>
                    gp.GameSessionId == sessionId &&
                    gp.UserId == userId);

        public Task<Domain.GamePlayer.GamePlayer?> GetOpponentAsync(
            Guid sessionId,
            Guid excludePlayerId)
            => Query()
                .FirstOrDefaultAsync(gp =>
                    gp.GameSessionId == sessionId &&
                    gp.UserId != excludePlayerId);

        public async Task<IReadOnlyList<Domain.GamePlayer.GamePlayer>> GetPlayersBySessionAsync(
            Guid sessionId)
            => await Query()
                .Where(gp => gp.GameSessionId == sessionId)
                .ToListAsync();

        public async Task<IReadOnlyList<Domain.GamePlayer.GamePlayer>> GetExpiredPlayersAsync(DateTimeOffset now, TimeSpan disconnectTimeout)
        {
            return await Query()
                .Where(gp =>
                    !gp.IsConnected &&
                    gp.LastConnectedAt != null &&
                    now - gp.LastConnectedAt > disconnectTimeout)
                .ToListAsync();
        }

        public  Task<Domain.GamePlayer.GamePlayer?> GetByIdAsync(Guid id)
            =>  Query()
                .FirstOrDefaultAsync(gp => gp.Id == id);
    }
}
