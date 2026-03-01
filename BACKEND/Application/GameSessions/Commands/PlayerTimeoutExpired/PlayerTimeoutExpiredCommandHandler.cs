using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.GameSessions.Services.GameSessionSnapshotFactory;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GamePlayer;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.GameSession;
using Domain.GamePlayer;
using Domain.GameSession;
using MediatR;


namespace Application.GameSessions.Commands.PlayerTimeoutExpired
{
    public class PlayerTimeoutExpiredCommandHandler : IRequestHandler<PlayerTimeoutExpiredCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGamePlayerReadRepository _playerReadRepo;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionSnapshotFactory _gameSessionSnapshotFactory;

        public PlayerTimeoutExpiredCommandHandler(
            IUnitOfWork uow,
            IGamePlayerReadRepository playerReadRepo,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider,
            IGameSessionSnapshotFactory gameSessionSnapshotFactory)
        {
            _uow = uow;
            _playerReadRepo = playerReadRepo;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
            _gameSessionSnapshotFactory = gameSessionSnapshotFactory;
        }

        public async Task<Unit> Handle(
            PlayerTimeoutExpiredCommand request,
            CancellationToken cancellationToken)
        {
            var now = _timeProvider.UtcNow;

            var player = await _playerReadRepo
                .GetByIdAsync(request.GamePlayerId, cancellationToken)
                .GetOrThrowAsync(nameof(GamePlayer), request.GamePlayerId);

            // Ha már visszacsatlakozott, nincs teendő
            if (player.IsConnected)
            {
                return Unit.Value;
            }

            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(player.GameSessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), player.GameSessionId);

            if (!session.CanTimeoutPlayer())
            {
                return Unit.Value;
            }

            var opponent = await _playerReadRepo
                .GetOpponentAsync(session.Id, player.Id, cancellationToken);

            // NINCS ellenfél -> abandoned
            if (opponent == null)
            {
                session.Abandon(now);
            }
            else if (opponent.IsConnected)
            {
                // Az ellenfél még játékban van -> ő nyer
                session.Finish(GameFinishReason.Timeout, opponent.Id, now);
            }
            else
            {
                // Mindkét játékos disconnected → abandoned
                session.Abandon(now);
            }

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionNotifier.SessionUpdated(
                session.Id,
                new SessionUpdatedMessage
                {
                    EventType = session.CurrentPhase == GamePhase.GameFinished
                        ? SessionEventType.SessionFinished
                        : SessionEventType.SessionAbandoned,
                    Snapshot = _gameSessionSnapshotFactory.Create(session)
                });

            return Unit.Value;
        }
    }
}
