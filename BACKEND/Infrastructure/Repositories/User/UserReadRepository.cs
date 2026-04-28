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

        public async Task<bool> ExistsByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken)
            => await Query()
                .AnyAsync(u => u.EmailAddress == emailAddress, cancellationToken);

        public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken)
            => await Query()
                .AnyAsync(u => u.UserName == userName, cancellationToken);

        public async Task<Domain.User.User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
            => await Query()
                .Include(u => u.AppRole)
                .FirstOrDefaultAsync(u => u.EmailAddress == email, cancellationToken);

        public async Task<Domain.User.User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
            => await Query()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        public async Task<Domain.User.User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
            => await Query()
                .Include(u => u.AppRole)
                .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }
}
