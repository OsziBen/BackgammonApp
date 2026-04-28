namespace Application.Interfaces.Repository.GroupRole
{
    public interface IGroupRoleReadRepository
    {
        Task<Domain.GroupRole.GroupRole?> GetBySystemNameAsync(string systemName, CancellationToken cancellationToken);
    }
}
