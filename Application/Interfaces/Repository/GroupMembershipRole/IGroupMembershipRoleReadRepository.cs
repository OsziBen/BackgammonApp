namespace Application.Interfaces.Repository.GroupMembershipRole
{
    public interface IGroupMembershipRoleReadRepository
    {
        Task<Domain.GroupMembershipRole.GroupMembershipRole?> GetByIdsAsync(Guid groupMembershipId, Guid groupRoleId);
    }
}
