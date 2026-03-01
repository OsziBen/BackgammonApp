using Application.Interfaces.Repository.AppRole;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AppRole
{
    public class AppRoleReadRepository
        : ReadRepositoryBase<Domain.AppRole.AppRole>,
        IAppRoleReadRepository
    {
        public AppRoleReadRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Domain.AppRole.AppRole?> GetByNameAsync(string name, CancellationToken cancellationToken)
            => await Query()
                .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }
}
