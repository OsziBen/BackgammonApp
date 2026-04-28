namespace Application.Interfaces.Repository.GroupMembership
{
    public interface IGroupMembershipReadRepository
    {
        Task<string?> GetUserRoleAsync(Guid userId, Guid groupId, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid userId, Guid groupId, CancellationToken cancellationToken);
        Task<List<Domain.GroupMembership.GroupMembership>> GetUsersByGroupIdAsync(Guid groupId, CancellationToken cancellationToken);
        Task<Domain.GroupMembership.GroupMembership?> GetAsync(Guid userId, Guid groupId, CancellationToken cancellationToken);
    }
}
