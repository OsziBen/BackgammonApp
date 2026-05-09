using Application.GameSessions.Services.GameSessionBroadcaster;
using Application.GameSessions.Services.GameSessionSnapshotFactory;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.AppRole;
using Application.Interfaces.Repository.GamePlayer;
using Application.Interfaces.Repository.GameSession;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupJoinRequest;
using Application.Interfaces.Repository.GroupMembership;
using Application.Interfaces.Repository.GroupMembershipRole;
using Application.Interfaces.Repository.GroupRole;
using Application.Interfaces.Repository.RulesTemplate;
using Application.Interfaces.Repository.Tournament;
using Application.Interfaces.Repository.TournamentJoinRequest;
using Application.Interfaces.Repository.TournamentParticipant;
using Application.Interfaces.Repository.User;
using Application.Shared.Time;
using Domain.GameLogic;
using Domain.GameSession.Services;
using Infrastructure.BackgroundServices;
using Infrastructure.Realtime.Factories;
using Infrastructure.Repositories;
using Infrastructure.Repositories.AppRole;
using Infrastructure.Repositories.GamePlayer;
using Infrastructure.Repositories.GameSession;
using Infrastructure.Repositories.Group;
using Infrastructure.Repositories.GroupJoinRequest;
using Infrastructure.Repositories.GroupMembership;
using Infrastructure.Repositories.GroupMembershipRole;
using Infrastructure.Repositories.GroupRole;
using Infrastructure.Repositories.RulesTemplate;
using Infrastructure.Repositories.Tournament;
using Infrastructure.Repositories.TournamentJoinRequest;
using Infrastructure.Repositories.TournamentParticipant;
using Infrastructure.Repositories.User;
using Infrastructure.Services;
using Infrastructure.Shared.Time;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExtensionMethods
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services)
        {
            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // GameSession
            services.AddScoped<IGameSessionReadRepository, GameSessionReadRepository>();
            services.AddScoped<IGameSessionWriteRepository, GameSessionWriteRepository>();

            // GamePlayer
            services.AddScoped<IGamePlayerReadRepository, GamePlayerReadRepository>();
            services.AddScoped<IGamePlayerWriteRepository, GamePlayerWriteRepository>();

            // User
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();

            // AppRole
            services.AddScoped<IAppRoleReadRepository, AppRoleReadRepository>();

            // Group
            services.AddScoped<IGroupReadRepository, GroupReadRepository>();
            services.AddScoped<IGroupWriteRepository, GroupWriteRepository>();

            // GroupJoinRequest
            services.AddScoped<IGroupJoinRequestReadRepository, GroupJoinRequestReadRepository>();
            services.AddScoped<IGroupJoinRequestWriteRepository, GroupJoinRequestWriteRepository>();

            // GroupRole
            services.AddScoped<IGroupRoleReadRepository, GroupRoleReadRepository>();

            // GroupMembership
            services.AddScoped<IGroupMembershipReadRepository, GroupMembershipReadRepository>();
            services.AddScoped<IGroupMembershipWriteRepository, GroupMembershipWriteRepository>();

            // GroupMembershipRole
            services.AddScoped<IGroupMembershipRoleReadRepository, GroupMembershipRoleReadRepository>();
            services.AddScoped<IGroupMembershipRoleWriteRepository, GroupMembershipRoleWriteRepository>();

            // Tournament
            services.AddScoped<ITournamentReadRepository, TournamentReadRepository>();
            services.AddScoped<ITournamentWriteRepository, TournamentWriteRepository>();

            // TournamentJoinRequests
            services.AddScoped<ITournamentJoinRequestReadRepository, TournamentJoinRequestReadRepository>();
            services.AddScoped<ITournamentJoinRequestWriteRepository, TournamentJoinRequestWriteRepository>();

            // TournamentParticipants
            services.AddScoped<ITournamentParticipantReadRepository, TournamentParticipantReadRepository>();
            services.AddScoped<ITournamentParticipantWriteRepository, TournamentParticipantWriteRepository>();

            // RulesTemplates
            services.AddScoped<IRulesTemplateReadRepository, RulesTemplateReadRepository>();

            return services;
        }

        public static IServiceCollection AddSystemServices(
            this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton<IDiceService, DiceService>();
            services.AddSingleton<IDateTimeProvider, SystemdateTimeProvider>();

            services.AddScoped<IBoardStateFactory, BoardStateFactory>();
            services.AddScoped<IGameSessionSnapshotFactory, GameSessionSnapshotFactory>();
            services.AddScoped<IDiceRoller, DiceRollerAdapter>();
            services.AddScoped<IStartingPlayerRoller, StartingPlayerRoller>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IGameSessionBroadcaster, GameSessionBroadcaster>();

            services.AddHostedService<DisconnectedPlayerMonitor>();

            return services;
        }
    }
}
