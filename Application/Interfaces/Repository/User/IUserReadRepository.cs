namespace Application.Interfaces.Repository.User
{
    public interface IUserReadRepository
    {
        Task<Domain.User.User?> GetByEmailAsync(string email);
        Task<bool> ExistsByUserNameAsync(string userName);
    }
}
