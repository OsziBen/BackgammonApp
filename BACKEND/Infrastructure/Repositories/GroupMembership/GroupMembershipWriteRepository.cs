using Application.Interfaces.Repository.GroupMembership;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GroupMembership
{
    public class GroupMembershipWriteRepository : IGroupMembershipWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupMembershipWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.GroupMembership.GroupMembership groupMembership, CancellationToken cancellationToken)
            => await _context.GroupMemberships.AddAsync(groupMembership, cancellationToken);

        public Task<Domain.GroupMembership.GroupMembership?> GetAnyAsync(Guid userId, Guid groupId, CancellationToken cancellationToken)
            => _context.GroupMemberships
                .Where(x => x.UserId == userId && x.GroupId == groupId)
                .OrderByDescending(x => x.IsActive)
                .ThenByDescending(x => x.JoinedAt)
                .FirstOrDefaultAsync(cancellationToken);

        public Task<Domain.GroupMembership.GroupMembership?> GetAsync(Guid userId, Guid groupId, CancellationToken cancellationToken)
            => _context.GroupMemberships
                .Include(x => x.GroupRoles)
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.GroupId == groupId &&
                    x.IsActive,
                    cancellationToken);

        public void Remove(Domain.GroupMembership.GroupMembership groupMembership)
            => _context.GroupMemberships.Remove(groupMembership);
    }
}
