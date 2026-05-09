using Application.Interfaces.Repository.TournamentJoinRequest;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.TournamentJoinRequest
{
    public class TournamentJoinRequestWriteRepository : ITournamentJoinRequestWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public TournamentJoinRequestWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.TournamentJoinRequest.TournamentJoinRequest tournamentJoinRequest, CancellationToken cancellationToken)
            => await _context.TournamentJoinRequests
                .AddAsync(tournamentJoinRequest, cancellationToken);

        public Task<Domain.TournamentJoinRequest.TournamentJoinRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => _context.TournamentJoinRequests
                .Include(tjr => tjr.User)
                .FirstOrDefaultAsync(tjr => tjr.Id == id, cancellationToken);
    }
}
