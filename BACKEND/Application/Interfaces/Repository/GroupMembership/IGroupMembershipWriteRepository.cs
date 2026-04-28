namespace Application.Interfaces.Repository.GroupMembership
{
    public interface IGroupMembershipWriteRepository
    {
        Task AddAsync(Domain.GroupMembership.GroupMembership groupMembership, CancellationToken cancellationToken);
        Task<Domain.GroupMembership.GroupMembership?> GetAsync(Guid userId, Guid groupId, CancellationToken cancellationToken);
        void Remove(Domain.GroupMembership.GroupMembership groupMembership);
        Task<Domain.GroupMembership.GroupMembership?> GetAnyAsync(Guid userId, Guid groupId, CancellationToken cancellationToken);
    }
}
