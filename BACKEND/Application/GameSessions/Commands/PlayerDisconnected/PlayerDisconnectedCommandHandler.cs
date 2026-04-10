using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.GameSessions.Services.GameSessionBroadcaster;
using Application.GameSessions.Services.GameSessionSnapshotFactory;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.GameSession;
using Domain.GamePlayer;
using MediatR;

namespace Application.GameSessions.Commands.PlayerDisconnected
{
    public class PlayerDisconnectedCommandHandler : IRequestHandler<PlayerDisconnectedCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionBroadcaster _gameSessionBroadcaster;

        public PlayerDisconnectedCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider,
            IGameSessionBroadcaster gameSessionBroadcaster)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _gameSessionBroadcaster = gameSessionBroadcaster;
        }

        public async Task<Unit> Handle(
            PlayerDisconnectedCommand request,
            CancellationToken cancellationToken)
        {
            var player = await _uow.GamePlayersWrite
                .GetByIdAsync(request.GamePlayerId, cancellationToken)
                .GetOrThrowAsync(nameof(GamePlayer), request.GamePlayerId);

            if (player == null)
            {
                return Unit.Value;
            }

            var now = _timeProvider.UtcNow;

            player.Disconnect(now);
            player.GameSession.MarkUpdated(now);
            player.GameSession.IncrementVersion();

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionBroadcaster.BroadcastAsync(player.GameSession, SessionEventType.PlayerDisconnected);

            return Unit.Value;
        }
    }
}
