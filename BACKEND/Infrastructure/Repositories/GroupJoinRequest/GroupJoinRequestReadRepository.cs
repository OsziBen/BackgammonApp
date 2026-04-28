using Application.Interfaces.Repository.GroupJoinRequest;
using Common.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GroupJoinRequest
{
    public class GroupJoinRequestReadRepository
        : ReadRepositoryBase<Domain.GroupJoinRequest.GroupJoinRequest>,
        IGroupJoinRequestReadRepository
    {
        public GroupJoinRequestReadRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Domain.GroupJoinRequest.GroupJoinRequest>> GetAllByGroupIdAsync(Guid groupId, CancellationToken cancellationToken)
            => await Query()
                .Where(gjr => gjr.GroupId == groupId)
                .Include(gjr => gjr.User)
                .Include(gjr => gjr.ReviewedByUser)
                .ToListAsync(cancellationToken);

        public async Task<List<Domain.GroupJoinRequest.GroupJoinRequest>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
            => await Query()
                .Where(gjr => gjr.UserId == userId)
                .Include(gjr => gjr.Group)
                .ToListAsync(cancellationToken);

        public Task<bool> HasPendingRequestAsync(Guid userId, Guid groupId, CancellationToken cancellationToken)
            => _context.GroupJoinRequests
                .AnyAsync(gjr =>
                gjr.UserId == userId &&
                gjr.GroupId == groupId &&
                gjr.Status == JoinRequestStatus.Pending,
                cancellationToken);
    }
}
