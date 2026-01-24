using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GamePlayer;
using Application.Interfaces.Repository.GameSession;
using Application.Interfaces.Repository.GroupMembershipRole;
using Application.Interfaces.Repository.User;
using Infrastructure.Data;
using Infrastructure.Repositories.GamePlayer;
using Infrastructure.Repositories.GameSession;
using Infrastructure.Repositories.GroupMembershipRole;
using Infrastructure.Repositories.User;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGameSessionWriteRepository GameSessionsWrite { get; }
        public IGamePlayerWriteRepository GamePlayersWrite { get; }
        public IUserWriteRepository UsersWrite { get; }
        public IGroupMembershipRoleWriteRepository GroupMembershipRolesWrite { get; }
        // repositories

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            GameSessionsWrite = new GameSessionWriteRepository(context);
            GamePlayersWrite = new GamePlayerWriteRepository(context);
            UsersWrite = new UserWriteRepository(context);
            GroupMembershipRolesWrite = new GroupMembershipRoleWriteRepository(context);
            // repositories
        }

        public Task<int> CommitAsync()
            => _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
