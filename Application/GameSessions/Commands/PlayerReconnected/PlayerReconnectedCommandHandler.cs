using Application.GameSessions.Realtime;
using Application.Interfaces.Repository;
using Application.Shared.Time;
using MediatR;

namespace Application.GameSessions.Commands.PlayerReconnected
{
    public class PlayerReconnectedCommandHandler : IRequestHandler<PlayerReconnectedCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;

        public PlayerReconnectedCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(
            PlayerReconnectedCommand request,
            CancellationToken cancellationToken)
        {
            var player = await _uow.GamePlayersWrite
                .GetByIdAsync(request.GamePlayerId);

            if (player == null || player.IsConnected)
            {
                return Unit.Value;
            }

            var now = _timeProvider.UtcNow;

            player.IsConnected = true;
            player.LastConnectedAt = now;

            await _uow.CommitAsync();

            await _gameSessionNotifier.PlayerReconnected(
                player.GameSessionId,
                player.Id,
                now);

            return Unit.Value;
        }
    }
}
