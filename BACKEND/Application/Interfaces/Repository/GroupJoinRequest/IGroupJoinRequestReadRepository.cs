namespace Application.Interfaces.Repository.GroupJoinRequest
{
    public interface IGroupJoinRequestReadRepository
    {
        Task<bool> HasPendingRequestAsync(Guid userId, Guid groupId, CancellationToken cancellationToken);
        Task<List<Domain.GroupJoinRequest.GroupJoinRequest>> GetAllByGroupIdAsync(Guid groupId, CancellationToken cancellationToken);
        Task<List<Domain.GroupJoinRequest.GroupJoinRequest>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
