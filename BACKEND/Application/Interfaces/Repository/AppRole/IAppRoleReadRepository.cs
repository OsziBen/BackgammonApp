namespace Application.Interfaces.Repository.AppRole
{
    public interface IAppRoleReadRepository
    {
        Task<Domain.AppRole.AppRole?> GetByNameAsync(
            string name,
            CancellationToken cancellationToken);
    }
}
