using Application.Interfaces.Repository.User;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.User
{
    public class UserWriteRepository : IUserWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public UserWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Domain.User.User?> GetByIdAsync(Guid id)
            => _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
    }
}
