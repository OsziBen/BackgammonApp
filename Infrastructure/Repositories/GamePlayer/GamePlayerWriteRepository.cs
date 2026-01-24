using Application.Interfaces.Repository.GamePlayer;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GamePlayer
{
    public class GamePlayerWriteRepository : IGamePlayerWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public GamePlayerWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Domain.GamePlayer.GamePlayer player)
            => _context.GamePlayers.AddAsync(player).AsTask();

        public Task<Domain.GamePlayer.GamePlayer?> GetByIdAsync(Guid id)
            => _context.GamePlayers
                .FirstOrDefaultAsync(gs => gs.Id == id);

        public Task<Domain.GamePlayer.GamePlayer?> GetBySessionAndUserAsync(Guid sessionId, Guid userId)
            => _context.GamePlayers
                .FirstOrDefaultAsync(gp =>
                    gp.GameSessionId == sessionId &&
                    gp.UserId == userId);

        public void Remove(Domain.GamePlayer.GamePlayer player)
            => _context.GamePlayers.Remove(player);

        public void Update(Domain.GamePlayer.GamePlayer player)
            => _context.GamePlayers.Update(player);
    }
}
