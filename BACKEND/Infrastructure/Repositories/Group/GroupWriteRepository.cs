using Application.Interfaces.Repository.Group;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Group
{
    public class GroupWriteRepository : IGroupWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.Group.Group group, CancellationToken cancellationToken)
            => await _context.Groups.AddAsync(group, cancellationToken);

        public Task<Domain.Group.Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => _context.Groups
                .Include(g => g.Creator)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }
}
