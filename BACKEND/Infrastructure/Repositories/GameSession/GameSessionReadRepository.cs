using Application.Interfaces.Repository.GameSession;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GameSession
{
    public class GameSessionReadRepository
        : ReadRepositoryBase<Domain.GameSession.GameSession>,
        IGameSessionReadRepository
    {
        public GameSessionReadRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<bool> ExistsBySessionCodeAsync(string sessionCode, CancellationToken cancellationToken)
            => Query()
                .AnyAsync(gs =>
                    !gs.IsFinished &&
                    gs.SessionCode == sessionCode, cancellationToken);

        public Task<Domain.GameSession.GameSession?> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken)
            => Query()
                .Include(gs => gs.Players)
                .FirstOrDefaultAsync(gs =>
                    !gs.IsDeleted &&
                    !gs.IsFinished &&
                    gs.Players.Any(p => p.UserId == userId), cancellationToken);

        public Task<Domain.GameSession.GameSession?> GetByIdAsync(Guid id)
            => Query()
                .Include(gs => gs.Players)
                .FirstOrDefaultAsync(gs => gs.Id == id);

        public Task<Domain.GameSession.GameSession?> GetBySessionCodeAsync(string sessionCode, CancellationToken cancellationToken, bool includePlayers = false)
        {
            IQueryable<Domain.GameSession.GameSession> query = Query();

            if (includePlayers)
            {
                query = query.Include(gs => gs.Players);
            }

            return query.FirstOrDefaultAsync(gs => gs.SessionCode == sessionCode, cancellationToken);
        }

        public Task<bool> HasActiveSession(Guid playerId, CancellationToken cancellationToken)
            => Query()
                .AnyAsync(gs =>
                    !gs.IsFinished &&
                    gs.Players.Any(p => p.Id == playerId), cancellationToken);
    }
}
