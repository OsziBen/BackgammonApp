using Application.Interfaces.Repository.TournamentJoinRequest;
using Common.Enums.Group;
using Domain.User;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.TournamentJoinRequest
{
    public class TournamentJoinRequestReadRepository
        : ReadRepositoryBase<Domain.TournamentJoinRequest.TournamentJoinRequest>,
        ITournamentJoinRequestReadRepository
    {
        public TournamentJoinRequestReadRepository(ApplicationDbContext context) : base(context) { }

        public Task<List<Domain.TournamentJoinRequest.TournamentJoinRequest>> GetAllByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken)
            => Query()
                .Where(tjr => tjr.TournamentId == tournamentId)
                .Include(tjr => tjr.User)
                .Include(tjr => tjr.ReviewedByUser)
                .ToListAsync(cancellationToken);

        public Task<List<Domain.TournamentJoinRequest.TournamentJoinRequest>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
            => Query()
                .Where(tjr => tjr.UserId == userId)
                .Include(tjr => tjr.Tournament)
                .ThenInclude(t => t.OrganizerUser)
                .ToListAsync(cancellationToken);

        public Task<bool> HasPendingRequestAsync(Guid userId, Guid tournamentId, CancellationToken cancellationToken)
            => Query()
                .AnyAsync(tjr =>
                    tjr.UserId == userId &&
                    tjr.TournamentId == tournamentId &&
                    tjr.Status == JoinRequestStatus.Pending,
                cancellationToken);
    }
}
