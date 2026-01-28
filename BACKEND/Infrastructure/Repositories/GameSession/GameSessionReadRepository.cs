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

        public Task<Domain.GameSession.GameSession?> GetByIdAsync(Guid id)
            => Query()
                .Include(gs => gs.Players)
                .FirstOrDefaultAsync(gs => gs.Id == id);

        public Task<Domain.GameSession.GameSession?> GetBySessionCodeAsync(string sessionCode)
            => Query()
                .Include(gs => gs.Players)
                .FirstOrDefaultAsync(gs => gs.SessionCode == sessionCode);

        public Task<bool> HasActiveSession(Guid playerId)
            => Query()
                .AnyAsync(gs =>
                    !gs.IsFinished &&
                    gs.Players.Any(p => p.Id == playerId));
    }
}
