using Application.Interfaces.Repository.GroupMembership;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GroupMembership
{
    public class GroupMembershipReadRepository
        : ReadRepositoryBase<Domain.GroupMembership.GroupMembership>,
        IGroupMembershipReadRepository
    {
        public GroupMembershipReadRepository(ApplicationDbContext context) : base(context) { }

        public Task<bool> ExistsAsync(Guid userId, Guid groupId, CancellationToken cancellationToken)
            => _context.GroupMemberships
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.GroupId == groupId &&
                    x.IsActive,
                    cancellationToken);

        public Task<Domain.GroupMembership.GroupMembership?> GetAsync(Guid userId, Guid groupId, CancellationToken cancellationToken)
            => _context.GroupMemberships
            .Include(x => x.Group)
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.GroupId == groupId &&
                x.IsActive,
                cancellationToken);

        public async Task<List<Domain.GroupMembership.GroupMembership>> GetMembershipsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
            => await Query()
                .Where(x => x.UserId == userId && x.IsActive)
                .ToListAsync(cancellationToken);

        public async Task<string?> GetUserRoleAsync(Guid userId, Guid groupId, CancellationToken cancellationToken)
            => await Query()
                .Where(gm => gm.UserId == userId && gm.GroupId == groupId)
                .SelectMany(gm => gm.GroupRoles)
                .Where(gmr => gmr.IsActive)
                .Select(gmr => gmr.GroupRole.SystemName)
                .FirstOrDefaultAsync(cancellationToken);

        public async Task<List<Domain.GroupMembership.GroupMembership>> GetUsersByGroupIdAsync(Guid groupId, CancellationToken cancellationToken)
            => await Query()
                .Where(x => x.GroupId == groupId && x.IsActive)
                .Include(x => x.User)
                .Include(x => x.GroupRoles)
                    .ThenInclude(gr => gr.GroupRole)
                .Include(x => x.GroupRoles)
                    .ThenInclude(gr => gr.GrantedByUser)
                .ToListAsync(cancellationToken);
    }
}
