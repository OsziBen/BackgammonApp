using Domain.GroupMembershipRole;

namespace Application.Interfaces
{
    public interface IGroupMembershipRoleRepository
    {
        Task<GroupMembershipRole?> GetAsync(
            Guid groupMembershipId,
            Guid groupRoleId,
            bool asNoTracking = true);

        Task AddAsync(GroupMembershipRole entity);
        void Remove(GroupMembershipRole entity);
    }
}
