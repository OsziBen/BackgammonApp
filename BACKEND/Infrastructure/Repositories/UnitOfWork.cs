using Application.Interfaces.Repository;
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
using Infrastructure.Data;
using Infrastructure.Repositories.GamePlayer;
using Infrastructure.Repositories.GameSession;
using Infrastructure.Repositories.Group;
using Infrastructure.Repositories.GroupJoinRequest;
using Infrastructure.Repositories.GroupMembership;
using Infrastructure.Repositories.GroupMembershipRole;
using Infrastructure.Repositories.Tournament;
using Infrastructure.Repositories.TournamentJoinRequest;
using Infrastructure.Repositories.TournamentParticipant;
using Infrastructure.Repositories.User;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGameSessionWriteRepository GameSessionsWrite { get; }
        public IGamePlayerWriteRepository GamePlayersWrite { get; }
        public IUserWriteRepository UsersWrite { get; }
        public IGroupWriteRepository GroupsWrite { get; }
        public IGroupJoinRequestWriteRepository GroupJoinRequestsWrite { get; }
        public IGroupMembershipWriteRepository GroupMembershipsWrite { get; }
        public IGroupMembershipRoleWriteRepository GroupMembershipRolesWrite { get; }
        public ITournamentWriteRepository TournamentsWrite { get; }
        public ITournamentJoinRequestWriteRepository TournamentJoinRequestsWrite { get; }
        public ITournamentParticipantWriteRepository TournamentParticipantsWrite { get; }

        // repositories

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            GameSessionsWrite = new GameSessionWriteRepository(context);
            GamePlayersWrite = new GamePlayerWriteRepository(context);
            UsersWrite = new UserWriteRepository(context);
            GroupsWrite = new GroupWriteRepository(context);
            GroupJoinRequestsWrite = new GroupJoinRequestWriteRepository(context);
            GroupMembershipsWrite = new GroupMembershipWriteRepository(context);
            GroupMembershipRolesWrite = new GroupMembershipRoleWriteRepository(context);
            TournamentsWrite = new TournamentWriteRepository(context);
            TournamentJoinRequestsWrite = new TournamentJoinRequestWriteRepository(context);
            TournamentParticipantsWrite = new TournamentParticipantWriteRepository(context);
            // repositories
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken)
            => _context.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
