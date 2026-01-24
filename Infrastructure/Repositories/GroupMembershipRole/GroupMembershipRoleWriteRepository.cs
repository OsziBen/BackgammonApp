using Application.Interfaces.Repository.GroupMembershipRole;
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

        public async Task AddAsync(Domain.GroupMembershipRole.GroupMembershipRole entity)
            => await _context.GroupMembershipRoles.AddAsync(entity);

        public Task<Domain.GroupMembershipRole.GroupMembershipRole?> GetByIdsAsync(Guid groupMembershipId, Guid groupRoleId)
            => _context.GroupMembershipRoles
                .FirstOrDefaultAsync(gmr =>
                    gmr.GroupMembershipId == groupMembershipId &&
                    gmr.GroupRoleId == groupRoleId);

        public void Remove(Domain.GroupMembershipRole.GroupMembershipRole entity)
            => _context.GroupMembershipRoles.Remove(entity);
    }
}
