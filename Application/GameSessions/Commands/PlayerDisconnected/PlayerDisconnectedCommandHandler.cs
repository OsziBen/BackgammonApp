using Application.GameSessions.Realtime;
using Application.Interfaces;
using MediatR;

namespace Application.GameSessions.Commands.PlayerDisconnected
{
    public class PlayerDisconnectedCommandHandler : IRequestHandler<PlayerDisconnectedCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _notifier;

        public PlayerDisconnectedCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier notifier)
        {
            _uow = uow;
            _notifier = notifier;
        }

        public async Task<Unit> Handle(
            PlayerDisconnectedCommand request,
            CancellationToken cancellationToken)
        {
            var player = await _uow.GamePlayers
                .GetByIdAsync(
                    request.GamePlayerId,
                    asNoTracking: false);

            if (player == null)
            {
                return Unit.Value;
            }

            player.IsConnected = false;
            player.LastConnectedAt = DateTimeOffset.UtcNow;

            await _uow.CommitAsync();

            await _notifier.PlayerDisconnected(
                player.GameSessionId,
                player.Id,
                player.LastConnectedAt.Value);

            return Unit.Value;
        }
    }
}
