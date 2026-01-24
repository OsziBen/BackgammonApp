namespace Application.Interfaces.Repository.GroupMembershipRole
{
    public interface IGroupMembershipRoleWriteRepository
    {
        Task<Domain.GroupMembershipRole.GroupMembershipRole?> GetByIdsAsync(Guid groupMembershipId, Guid groupRoleId);
        Task AddAsync(Domain.GroupMembershipRole.GroupMembershipRole entity);
        void Remove(Domain.GroupMembershipRole.GroupMembershipRole entity);
    }
}
