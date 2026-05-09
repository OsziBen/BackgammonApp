using Application.Interfaces.Repository.GroupJoinRequest;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.GroupJoinRequest
{
    public class GroupJoinRequestWriteRepository : IGroupJoinRequestWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupJoinRequestWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.GroupJoinRequest.GroupJoinRequest groupJoinRequest, CancellationToken cancellationToken)
            => await _context.GroupJoinRequests
                .AddAsync(groupJoinRequest, cancellationToken);

        public Task<Domain.GroupJoinRequest.GroupJoinRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => _context.GroupJoinRequests
                .FirstOrDefaultAsync(gjr => gjr.Id == id, cancellationToken);
    }
}
