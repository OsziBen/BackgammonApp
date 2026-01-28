using Application.Interfaces.Repository.User;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.User
{
    public class UserReadRepository
        : ReadRepositoryBase<Domain.User.User>,
        IUserReadRepository
    {
        public UserReadRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExistsByUserNameAsync(string userName)
            => await Query()
                .AnyAsync(u => u.UserName == userName);

        public async Task<Domain.User.User?> GetByEmailAsync(string email)
            => await Query()
                .FirstOrDefaultAsync(u => u.EmailAddress == email);
    }
}
