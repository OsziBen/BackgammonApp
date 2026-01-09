using Application.Interfaces;
using Domain.GameSession;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GameSessionRepository : BaseRepository<GameSession>, IGameSessionRepository
    {
        public GameSessionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<GameSession?> GetBySessionCodeAsync(
            string sessionCode,
            bool includePlayers = true,
            bool asNoTracking = false)
        {
            IQueryable<GameSession> query = Query(asNoTracking: asNoTracking);

            if (includePlayers)
            {
                query = query.Include(gs => gs.Players);
            }

            return await query.FirstOrDefaultAsync(gs => gs.SessionCode == sessionCode);
        }

        public async Task<bool> HasActiveSession(Guid playerId)
        {
            return await Query(asNoTracking: true)
                .AnyAsync(gs =>
                !gs.IsFinished &&
                gs.Players.Any(p => p.Id == playerId));
        }
    }
}
