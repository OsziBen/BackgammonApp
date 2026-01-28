using Application.Interfaces.Repository.GroupMembershipRole;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GroupMembershipRole
{
    public class GroupMembershipRoleReadRepository : IGroupMembershipRoleReadRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupMembershipRoleReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Domain.GroupMembershipRole.GroupMembershipRole?> GetByIdsAsync(
            Guid groupMembershipId,
            Guid groupRoleId)
        {
            var query = _context.GroupMembershipRoles.AsQueryable()
                .AsNoTracking();

            return query.FirstOrDefaultAsync(x =>
                x.GroupMembershipId == groupMembershipId &&
                x.GroupRoleId == groupRoleId);
        }
    }
}
