using Application.Interfaces.Repository.TournamentParticipant;
using Common.Enums.TournamentParticipant;
using Domain.GroupMembershipRole;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.TournamentParticipant
{
    public class TournamentParticipantWriteRepository : ITournamentParticipantWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public TournamentParticipantWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.TournamentParticipant.TournamentParticipant tournamentParticipant, CancellationToken cancellationToken)
            => await _context.TournamentParticipants.AddAsync(tournamentParticipant, cancellationToken);

        public Task<Domain.TournamentParticipant.TournamentParticipant?> GetAsync(Guid userId, Guid tournamentId, CancellationToken cancellationToken)
            => _context.TournamentParticipants
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.TournamentId == tournamentId &&
                    x.Status == TournamentParticipantStatus.Active,
                    cancellationToken);
    }
}
