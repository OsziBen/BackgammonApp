namespace Application.Interfaces.Repository.GroupJoinRequest
{
    public interface IGroupJoinRequestWriteRepository
    {
        Task AddAsync(Domain.GroupJoinRequest.GroupJoinRequest groupJoinRequest, CancellationToken cancellationToken);
        Task<Domain.GroupJoinRequest.GroupJoinRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
