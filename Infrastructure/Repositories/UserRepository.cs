using Application.Interfaces;
using Domain.User;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository :
        BaseRepository<User>,
        IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<bool> ExistsByUserNameAsync(string userName)
            => await Query()
                .AnyAsync(u => u.UserName == userName);


        public async Task<User?> GetByEmailAsync(string email)
            => await Query()
                .FirstOrDefaultAsync(u => u.EmailAddress == email);
    }
}
