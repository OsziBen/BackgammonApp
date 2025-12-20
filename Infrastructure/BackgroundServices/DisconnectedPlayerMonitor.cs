using Application.GameSessions.Commands.PlayerTimeoutExpired;
using Application.Interfaces;
using Common.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundServices
{
    public class DisconnectedPlayerMonitor : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DisconnectedPlayerMonitor(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var uow = scope.ServiceProvider
                    .GetRequiredService<IUnitOfWork>();

                var mediator = scope.ServiceProvider
                    .GetRequiredService<IMediator>();

                var now = DateTimeOffset.UtcNow;

                var expiredPlayers = await uow.GamePlayers
                    .Query(asNoTracking: false)
                    .Where(p =>
                        !p.IsConnected &&
                        p.LastConnectedAt != null &&
                        now - p.LastConnectedAt >
                            GameSessionConstants.DisconnectTimeout)
                    .ToListAsync(stoppingToken);

                foreach (var player in expiredPlayers)
                {
                    await mediator.Send(new PlayerTimeoutExpiredCommand(player.Id), stoppingToken);
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(5),
                    stoppingToken);
            }
        }
    }
}
