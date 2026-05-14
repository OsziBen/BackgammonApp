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
                .Include(gp => gp.User)
                .FirstOrDefaultAsync(gp =>
                    gp.GameSessionId == sessionId &&
                    gp.UserId == userId);

        public Task<Domain.GamePlayer.GamePlayer?> GetOpponentAsync(
            Guid sessionId,
            Guid excludePlayerId,
            CancellationToken cancellationToken)
            => Query()
                .Include(gp => gp.User)
                .FirstOrDefaultAsync(gp =>
                    gp.GameSessionId == sessionId &&
                    gp.UserId != excludePlayerId,
                cancellationToken);

        public async Task<IReadOnlyList<Domain.GamePlayer.GamePlayer>> GetPlayersBySessionAsync(
            Guid sessionId)
            => await Query()
                .Include(gp => gp.User)
                .Where(gp => gp.GameSessionId == sessionId)
                .ToListAsync();

        public async Task<IReadOnlyList<Domain.GamePlayer.GamePlayer>> GetExpiredPlayersAsync(
            DateTimeOffset now,
            TimeSpan disconnectTimeout,
            CancellationToken cancellationToken)
        {
            return await Query()
                .Include(gp => gp.User)
                .Where(gp =>
                    !gp.IsConnected &&
                    gp.LastConnectedAt != null &&
                    now - gp.LastConnectedAt > disconnectTimeout &&
                    !gp.GameSession.IsFinished)
                .ToListAsync(cancellationToken);
        }

        public  Task<Domain.GamePlayer.GamePlayer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            =>  Query()
                .Include(gp => gp.User)
                .FirstOrDefaultAsync(gp => gp.Id == id, cancellationToken);
    }
}
