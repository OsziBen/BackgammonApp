using Application.Interfaces.Repository.GameSession;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GameSession
{
    public class GameSessionWriteRepository : IGameSessionWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public GameSessionWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.GameSession.GameSession session)
            => await _context.GameSessions.AddAsync(session);

        public Task<Domain.GameSession.GameSession?> GetByIdAsync(Guid id)
            => _context.GameSessions
                .Include(gs => gs.Players)
                .FirstOrDefaultAsync(gs => gs.Id == id);

        public Task<Domain.GameSession.GameSession?> GetBySessionCodeAsync(string sessionCode, bool includePlayers = false)
        {
            IQueryable<Domain.GameSession.GameSession> query = _context.GameSessions;

            if (includePlayers)
            {
                query = query.Include(gs => gs.Players);
            }

            return query.FirstOrDefaultAsync(gs => gs.SessionCode == sessionCode);
        }

        public void Update(Domain.GameSession.GameSession session)
            => _context.GameSessions.Update(session);
    }
}
