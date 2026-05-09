using Application.Interfaces.Repository.TournamentParticipant;
using Common.Enums.TournamentParticipant;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.TournamentParticipant
{
    public class TournamentParticipantReadRepository
        : ReadRepositoryBase<Domain.TournamentParticipant.TournamentParticipant>,
        ITournamentParticipantReadRepository
    {
        public TournamentParticipantReadRepository(ApplicationDbContext context) : base(context) { }

        public Task<bool> ExistsAsync(Guid userId, Guid tournamentId, CancellationToken cancellationToken)
            => Query()
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.TournamentId == tournamentId,
                cancellationToken);

        public Task<List<Domain.TournamentParticipant.TournamentParticipant>> GetUsersByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken)
            => Query()
                .Where(x => x.TournamentId == tournamentId && x.Status == TournamentParticipantStatus.Active)
                .Include(x => x.User)
                .Include(x => x.Tournament)
                .ToListAsync(cancellationToken);
    }
}
