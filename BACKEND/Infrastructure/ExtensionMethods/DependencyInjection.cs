using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GamePlayer;
using Application.Interfaces.Repository.GameSession;
using Application.Interfaces.Repository.GroupMembershipRole;
using Application.Interfaces.Repository.User;
using Application.Shared.Time;
using Domain.GameLogic;
using Domain.GameSession.Services;
using Infrastructure.BackgroundServices;
using Infrastructure.Realtime.Factories;
using Infrastructure.Repositories;
using Infrastructure.Repositories.GamePlayer;
using Infrastructure.Repositories.GameSession;
using Infrastructure.Repositories.GroupMembershipRole;
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

            // GroupMembershipRole
            services.AddScoped<IGroupMembershipRoleReadRepository, GroupMembershipRoleReadRepository>();
            services.AddScoped<IGroupMembershipRoleWriteRepository, GroupMembershipRoleWriteRepository>();

            return services;
        }

        public static IServiceCollection AddSystemServices(
            this IServiceCollection services)
        {
            services.AddSingleton<IDiceService, DiceService>();
            services.AddSingleton<IDateTimeProvider, SystemdateTimeProvider>();

            services.AddScoped<IBoardStateFactory, BoardStateFactory>();
            services.AddScoped<IDiceRoller, DiceRollerAdapter>();
            services.AddScoped<IStartingPlayerRoller, StartingPlayerRoller>();

            services.AddHostedService<DisconnectedPlayerMonitor>();

            return services;
        }
    }
}
