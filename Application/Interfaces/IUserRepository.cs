using Domain.User;

namespace Application.Interfaces
{
    public interface IUserRepository :
        IBaseRepository<User>, 
        IReadRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByUserNameAsync(string userName);
    }
}
