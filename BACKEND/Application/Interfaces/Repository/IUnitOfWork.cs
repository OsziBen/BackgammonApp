using Application.Interfaces.Repository.GamePlayer;
using Application.Interfaces.Repository.GameSession;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupJoinRequest;
using Application.Interfaces.Repository.GroupMembership;
using Application.Interfaces.Repository.GroupMembershipRole;
using Application.Interfaces.Repository.Tournament;
using Application.Interfaces.Repository.TournamentJoinRequest;
using Application.Interfaces.Repository.TournamentParticipant;
using Application.Interfaces.Repository.User;

namespace Application.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGameSessionWriteRepository GameSessionsWrite { get; }
        IGamePlayerWriteRepository GamePlayersWrite { get; }
        IUserWriteRepository UsersWrite { get; }
        IGroupWriteRepository GroupsWrite { get; }
        IGroupJoinRequestWriteRepository GroupJoinRequestsWrite { get; }
        IGroupMembershipWriteRepository GroupMembershipsWrite { get; }
        IGroupMembershipRoleWriteRepository GroupMembershipRolesWrite { get; }
        ITournamentWriteRepository TournamentsWrite { get; }
        ITournamentJoinRequestWriteRepository TournamentJoinRequestsWrite { get; }
        ITournamentParticipantWriteRepository TournamentParticipantsWrite { get; }
        // repositories

        Task<int> CommitAsync(CancellationToken cancellationToken);
    }
}
