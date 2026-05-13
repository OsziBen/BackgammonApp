using Application.Interfaces.Repository.Tournament;
using Common.Enums.Tournament;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Tournament
{
    public class TournamentReadRepository
        : ReadRepositoryBase<Domain.Tournament.Tournament>,
        ITournamentReadRepository
    {
        public TournamentReadRepository(ApplicationDbContext context) : base(context) { }

        public Task<bool> ExistsByNameAsync(string tournamentName, CancellationToken cancellationToken)
            => Query()
                .AnyAsync(t => t.Name == tournamentName, cancellationToken);

        public Task<List<Domain.Tournament.Tournament>> GetAllByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken)
            => Query()
                .Where(t =>
                    t.OrganizerUserId == userId
                    || t.Participants.Any(p => p.UserId == userId))
                .Include(t => t.OrganizerUser)
                .Include(t => t.Participants)
                .Include(t => t.RulesTemplate)
                .ToListAsync(cancellationToken);

        public Task<List<Domain.Tournament.Tournament>> GetAllPublicAsync(CancellationToken cancellationToken)
            => Query()
                .Where(t => t.Visibility == TournamentVisibility.Public)
                .Include(t => t.OrganizerUser)
                .Include(t => t.Participants)
                .Include(t => t.RulesTemplate)
                .ToListAsync(cancellationToken);

        public Task<Domain.Tournament.Tournament?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => Query()
                .Include(t => t.OrganizerUser)
                .Include(t => t.Participants)
                .Include(t => t.RulesTemplate)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}
