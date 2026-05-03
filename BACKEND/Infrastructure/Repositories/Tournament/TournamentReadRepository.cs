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

        public Task<List<Domain.Tournament.Tournament>> GetAllPublicAsync(CancellationToken cancellationToken)
            => Query()
                .Where(t => t.Visibility == TournamentVisibility.Public)
                .Include(t => t.OrganizerUser)
                .ToListAsync(cancellationToken);
    }
}
