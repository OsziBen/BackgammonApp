using Application.Interfaces.Repository.GamePlayer;
using Application.Interfaces.Repository.GameSession;
using Application.Interfaces.Repository.GroupMembershipRole;
using Application.Interfaces.Repository.User;

namespace Application.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGameSessionWriteRepository GameSessionsWrite { get; }
        IGamePlayerWriteRepository GamePlayersWrite { get; }
        IUserWriteRepository UsersWrite { get; }
        IGroupMembershipRoleWriteRepository GroupMembershipRolesWrite { get; }
        // repositories

        Task<int> CommitAsync();
    }
}
