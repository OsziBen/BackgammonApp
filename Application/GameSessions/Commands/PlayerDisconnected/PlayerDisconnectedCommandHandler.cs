using Application.GameSessions.Realtime;
using Application.Interfaces.Repository;
using Application.Shared.Time;
using MediatR;

namespace Application.GameSessions.Commands.PlayerDisconnected
{
    public class PlayerDisconnectedCommandHandler : IRequestHandler<PlayerDisconnectedCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _notifier;
        private readonly IDateTimeProvider _timeProvider;

        public PlayerDisconnectedCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier notifier,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _notifier = notifier;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(
            PlayerDisconnectedCommand request,
            CancellationToken cancellationToken)
        {
            var player = await _uow.GamePlayersWrite
                .GetByIdAsync(request.GamePlayerId);

            if (player == null)
            {
                return Unit.Value;
            }

            var now = _timeProvider.UtcNow;

            player.IsConnected = false;
            player.LastConnectedAt = now;

            await _uow.CommitAsync();

            await _notifier.PlayerDisconnected(
                player.GameSessionId,
                player.Id,
                now);

            return Unit.Value;
        }
    }
}
