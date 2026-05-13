using Application.Interfaces.Repository.GroupMembershipRole;
using Domain.GroupRole;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GroupMembershipRole
{
    public class GroupMembershipRoleWriteRepository : IGroupMembershipRoleWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupMembershipRoleWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.GroupMembershipRole.GroupMembershipRole groupMembershipRole, CancellationToken cancellationToken)
            => await _context.GroupMembershipRoles.AddAsync(groupMembershipRole, cancellationToken);

        public Task<int> CountActiveByRoleAsync(Guid groupId, string roleName, CancellationToken cancellationToken)
            => _context.GroupMembershipRoles
                .Where(x => x.IsActive)
                .Where(x => x.GroupMembership.GroupId == groupId)
                .Where(x => x.GroupRole.Name == roleName)
                .CountAsync(cancellationToken);

        public Task<bool> ExistsActiveRoleAsync(Guid membershipId, Guid roleId, CancellationToken cancellationToken)
            => _context.GroupMembershipRoles
                .AnyAsync(x =>
                    x.GroupMembershipId == membershipId &&
                    x.GroupRoleId == roleId &&
                    x.IsActive, cancellationToken);

        public Task<List<Domain.GroupMembershipRole.GroupMembershipRole>> GetActiveRolesAsync(Guid groupMembershipId, CancellationToken cancellationToken)
            => _context.GroupMembershipRoles
                .Where(gmr =>
                    gmr.GroupMembershipId == groupMembershipId &&
                    gmr.IsActive)
                .ToListAsync(cancellationToken);

        public Task<Domain.GroupMembershipRole.GroupMembershipRole?> GetByIdAsync(Guid groupMembershipId, CancellationToken cancellationToken)
            => _context.GroupMembershipRoles
                .FirstOrDefaultAsync(gmr =>
                    gmr.GroupMembershipId == groupMembershipId,
                cancellationToken);

        public Task<bool> IsOwnerAsync(Guid membershipId, CancellationToken cancellationToken)
            => _context.GroupMembershipRoles
                .AnyAsync(x =>
                    x.GroupMembershipId == membershipId &&
                    x.GroupRole.Name == GroupRoleConstants.Owner &&
                    x.IsActive,
                    cancellationToken);

        public void Remove(Domain.GroupMembershipRole.GroupMembershipRole entity)
            => _context.GroupMembershipRoles.Remove(entity);
    }
}
