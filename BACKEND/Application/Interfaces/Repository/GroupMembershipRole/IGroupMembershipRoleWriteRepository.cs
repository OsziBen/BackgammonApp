namespace Application.Interfaces.Repository.GroupMembershipRole
{
    public interface IGroupMembershipRoleWriteRepository
    {
        Task<Domain.GroupMembershipRole.GroupMembershipRole?> GetByIdAsync(Guid groupMembershipId, CancellationToken cancellationToken);
        Task AddAsync(Domain.GroupMembershipRole.GroupMembershipRole groupMembershipRole, CancellationToken cancellationToken);
        void Remove(Domain.GroupMembershipRole.GroupMembershipRole entity);
        Task<List<Domain.GroupMembershipRole.GroupMembershipRole>> GetActiveRolesAsync(Guid groupMembershipId, CancellationToken cancellationToken);
        Task<bool> ExistsActiveRoleAsync(Guid membershipId, Guid roleId, CancellationToken cancellationToken);
        Task<int> CountActiveByRoleAsync(Guid groupId, string roleName, CancellationToken cancellationToken);
        Task<bool> IsOwnerAsync(Guid membershipId, CancellationToken cancellationToken);
    }
}
