using Domain.AppRole;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IBaseRepository<AppRole> AppRoles { get; }
        IGroupMembershipRoleRepository GroupMembershipRoles { get; }
        // repositories

        IGameSessionRepository GameSessions { get; }
        IGamePlayerRepository GamePlayers { get; }

        Task<int> CommitAsync();
        Task RollbackAsync();
    }
}
