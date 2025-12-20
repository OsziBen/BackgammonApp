using Application.GameSessions.Realtime;
using Application.Realtime.Connections;
using Infrastructure.BackgroundServices;
using Infrastructure.Realtime.Connections;
using WebAPI.Realtime;

namespace WebAPI.Extensions
{
    public static class RealtimeExtensions
    {
        public static IServiceCollection AddRealtimeServices(this IServiceCollection services)
        {
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;

                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            });

            services.AddSingleton<IConnectionMapping, InMemoryConnectionMapping>();

            services.AddHostedService<DisconnectedPlayerMonitor>();

            services.AddScoped<IGameSessionNotifier, SignalRGameSessionNotifier>();

            return services;
        }
    }
}
