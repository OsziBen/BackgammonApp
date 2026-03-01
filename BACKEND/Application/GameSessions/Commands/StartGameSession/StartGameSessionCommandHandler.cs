using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.GameSessions.Services.GameSessionSnapshotFactory;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.GameSession;
using Domain.GameSession;
using Domain.GameSession.Services;
using MediatR;

namespace Application.GameSessions.Commands.StartGameSession
{
    public class StartGameSessionCommandHandler : IRequestHandler<StartGameSessionCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IGameSessionSnapshotFactory _gameSessionSnapshotFactory;
        private readonly IStartingPlayerRoller _startingPlayerRoller;

        public StartGameSessionCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider,
            IGameSessionNotifier gameSessionNotifier,
            IGameSessionSnapshotFactory gameSessionSnapshotFactory,
            IStartingPlayerRoller startingPlayerRoller)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _gameSessionNotifier = gameSessionNotifier;
            _gameSessionSnapshotFactory = gameSessionSnapshotFactory;
            _startingPlayerRoller = startingPlayerRoller;
        }

        public async Task<Unit> Handle(
            StartGameSessionCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            if (!session.CanStartGame())
            {
                return Unit.Value;
            }

            var now = _timeProvider.UtcNow;

            session.Start(now);

            session.DetermineStartingPlayer(_startingPlayerRoller, now);

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionNotifier.SessionUpdated(
                session.Id,
                new SessionUpdatedMessage
                {
                    EventType = SessionEventType.GameStarted,
                    Snapshot = _gameSessionSnapshotFactory.Create(session)
                });

            return Unit.Value;
        }
    }
}
