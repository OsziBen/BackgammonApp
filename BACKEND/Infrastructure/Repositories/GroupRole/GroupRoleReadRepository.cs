using Application.Interfaces.Repository.GroupRole;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GroupRole
{
    public class GroupRoleReadRepository
        : ReadRepositoryBase<Domain.GroupRole.GroupRole>,
        IGroupRoleReadRepository
    {
        public GroupRoleReadRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Domain.GroupRole.GroupRole?> GetBySystemNameAsync(string systemName, CancellationToken cancellationToken)
            => await Query()
                .FirstOrDefaultAsync(gr => gr.SystemName == systemName, cancellationToken);

    }
}
