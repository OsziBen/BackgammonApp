using Application.Interfaces;
using Domain.GroupMembershipRole;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GroupMembershipRoleRepository : IGroupMembershipRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<GroupMembershipRole> _dbSet;

        public GroupMembershipRoleRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<GroupMembershipRole>();
        }

        public async Task AddAsync(GroupMembershipRole entity)
            => await _dbSet.AddAsync(entity);

        public async Task<GroupMembershipRole?> GetAsync(
            Guid groupMembershipId,
            Guid groupRoleId,
            bool asNoTracking = true)
        {
            var query = _context.GroupMembershipRoles.AsQueryable();

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(x =>
                x.GroupMembershipId == groupMembershipId &&
                x.GroupRoleId == groupRoleId);
        }

        public void Remove(GroupMembershipRole entity)
            => _dbSet.Remove(entity);
    }
}
