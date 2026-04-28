namespace Application.Interfaces.Repository.Group
{
    public interface IGroupWriteRepository
    {
        Task<Domain.Group.Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Domain.Group.Group group, CancellationToken cancellationToken);
    }
}
