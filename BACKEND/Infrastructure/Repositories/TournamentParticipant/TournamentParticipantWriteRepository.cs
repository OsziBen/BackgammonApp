using Application.Interfaces.Repository.TournamentParticipant;
using Common.Enums.TournamentParticipant;
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

        public Task<Domain.TournamentParticipant.TournamentParticipant?> GetAsync(Guid userId, Guid tournamentId, CancellationToken cancellationToken)
            => _context.TournamentParticipants
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.TournamentId == tournamentId &&
                    x.Status == TournamentParticipantStatus.Active,
                    cancellationToken);
    }
}
