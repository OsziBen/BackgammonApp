using Application.GameSessions.Commands.PlayerTimeoutExpired;
using Application.Interfaces.Repository.GamePlayer;
using Application.Shared.Time;
using Common.Constants;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundServices
{
    public class DisconnectedPlayerMonitor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IDateTimeProvider _timeProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(5);

        public DisconnectedPlayerMonitor(
            IServiceScopeFactory scopeFactory,
            IDateTimeProvider timeProvider)
        {
            _scopeFactory = scopeFactory;
            _timeProvider = timeProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var playerRepo = scope.ServiceProvider.GetRequiredService<IGamePlayerReadRepository>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var now = _timeProvider.UtcNow;

                var expiredPlayers = await playerRepo
                    .GetExpiredPlayersAsync(now, GameSessionConstants.DisconnectTimeout);

                foreach (var player in expiredPlayers)
                {
                    await mediator.Send(
                        new PlayerTimeoutExpiredCommand(player.Id),
                        cancellationToken);
                }

                await Task.Delay(_checkInterval, cancellationToken);
            }
        }
    }
}
