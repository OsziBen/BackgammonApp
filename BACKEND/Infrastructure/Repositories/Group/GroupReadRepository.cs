using Application.Interfaces.Repository.Group;
using Common.Enums.Group;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Group
{
    public class GroupReadRepository
        : ReadRepositoryBase<Domain.Group.Group>,
        IGroupReadRepository
    {
        public GroupReadRepository(ApplicationDbContext context) : base(context) { }

        public Task<bool> ExistsByNameAsync(string groupName, CancellationToken cancellationToken)
            => Query()
                .AnyAsync(g => g.Name == groupName, cancellationToken);

        public Task<List<Domain.Group.Group>> GetAllPublicAsync(CancellationToken cancellationToken)
            => Query()
                .Where(g => g.Visibility == GroupVisibility.Public)
                .Include(g => g.Creator)
                .ToListAsync(cancellationToken);

        public Task<List<Domain.Group.Group>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
            => Query()
                .Where(g => g.CreatorId == userId)
                .Include(g => g.Creator)
                .ToListAsync(cancellationToken);

        public Task<Domain.Group.Group?> GetByIdAsync(Guid groupId, CancellationToken cancellationToken)
            => Query()
                .Include(g => g.Creator)
                .Include(g => g.GroupMemberships)
                .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
    }
}
