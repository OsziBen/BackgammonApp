namespace Application.Interfaces.Repository.User
{
    public interface IUserWriteRepository
    {
        Task<Domain.User.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Domain.User.User user, CancellationToken cancellationToken);
    }
}
