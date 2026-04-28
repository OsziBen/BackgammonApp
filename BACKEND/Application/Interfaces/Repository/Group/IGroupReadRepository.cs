namespace Application.Interfaces.Repository.Group
{
    public interface IGroupReadRepository
    {
        Task<bool> ExistsByNameAsync(string groupName, CancellationToken cancellationToken);
        Task<List<Domain.Group.Group>> GetAllPublicAsync(CancellationToken cancellationToken);
        Task<List<Domain.Group.Group>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Domain.Group.Group?> GetByIdAsync(Guid groupId, CancellationToken cancellationToken);
    }
}
