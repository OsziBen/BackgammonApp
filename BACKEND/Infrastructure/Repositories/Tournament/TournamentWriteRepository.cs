using Application.Interfaces.Repository.Tournament;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Tournament
{
    public class TournamentWriteRepository : ITournamentWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public TournamentWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.Tournament.Tournament tournament, CancellationToken cancellationToken)
            => await _context.Tournaments.AddAsync(tournament, cancellationToken);

        public Task<Domain.Tournament.Tournament?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => _context.Tournaments
                .Include(t => t.OrganizerUser)
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}
