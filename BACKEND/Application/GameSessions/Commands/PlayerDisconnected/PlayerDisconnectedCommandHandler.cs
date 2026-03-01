using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
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
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionSnapshotFactory _gameSessionSnapshotFactory;

        public PlayerDisconnectedCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider,
            IGameSessionSnapshotFactory gameSessionSnapshotFactory)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
            _gameSessionSnapshotFactory = gameSessionSnapshotFactory;
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

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionNotifier.SessionUpdated(
                player.GameSessionId,
                new SessionUpdatedMessage
                {
                    EventType = SessionEventType.PlayerDisconnected,
                    Snapshot = _gameSessionSnapshotFactory.Create(player.GameSession)
                });

            return Unit.Value;
        }
    }
}
