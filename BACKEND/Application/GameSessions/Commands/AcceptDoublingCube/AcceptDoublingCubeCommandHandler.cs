using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.GameSessions.Services.GameSessionSnapshotFactory;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.GameSession;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.AcceptDoublingCube
{
    public class AcceptDoublingCubeCommandHandler : IRequestHandler<AcceptDoublingCubeCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionSnapshotFactory _gameSessionSnapshotFactory;

        public AcceptDoublingCubeCommandHandler(
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

        public async Task<Unit> Handle(AcceptDoublingCubeCommand request, CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;

            var result = session.AcceptDoublingCube(
                request.PlayerId,
                now);

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionNotifier.SessionUpdated(
                session.Id,
                new SessionUpdatedMessage
                {
                    EventType = SessionEventType.DoubleAccepted,
                    Snapshot = _gameSessionSnapshotFactory.Create(session)
                });

            return Unit.Value;
        }
    }
}
