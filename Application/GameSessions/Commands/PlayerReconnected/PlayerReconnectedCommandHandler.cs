using Application.GameSessions.Realtime;
using Application.Interfaces;
using MediatR;

namespace Application.GameSessions.Commands.PlayerReconnected
{
    public class PlayerReconnectedCommandHandler : IRequestHandler<PlayerReconnectedCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;

        public PlayerReconnectedCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
        }

        public async Task<Unit> Handle(
            PlayerReconnectedCommand request,
            CancellationToken cancellationToken)
        {
            var player = await _uow.GamePlayers
                .GetByIdAsync(request.GamePlayerId, asNoTracking: false);

            if (player == null || player.IsConnected)
            {
                return Unit.Value;
            }

            player.IsConnected = true;
            player.LastConnectedAt = DateTime.UtcNow;

            await _uow.CommitAsync();

            await _gameSessionNotifier.PlayerReconnected(
                player.GameSessionId,
                player.Id,
                player.LastConnectedAt.Value);

            return Unit.Value;
        }
    }
}
