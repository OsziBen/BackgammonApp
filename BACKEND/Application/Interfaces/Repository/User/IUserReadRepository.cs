namespace Application.Interfaces.Repository.User
{
    public interface IUserReadRepository
    {
        Task<Domain.User.User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Domain.User.User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
        Task<Domain.User.User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken);
        Task<bool> ExistsByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken);
    }
}
