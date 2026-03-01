using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.GameSessions.Services.GameSessionSnapshotFactory;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.GameSession;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.DeclineDoublingCube
{
    public class DeclineDoublingCubeCommandHandler : IRequestHandler<DeclineDoublingCubeCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IBoardStateFactory _boardStateFactory;
        private readonly IGameSessionSnapshotFactory _gameSessionSnapshotFactory;

        public DeclineDoublingCubeCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider,
            IBoardStateFactory boardStateFactory,
            IGameSessionSnapshotFactory gameSessionSnapshotFactory)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
            _boardStateFactory = boardStateFactory;
            _gameSessionSnapshotFactory = gameSessionSnapshotFactory;
        }

        public async Task<Unit> Handle(DeclineDoublingCubeCommand request, CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;
            var boardState = _boardStateFactory.Create(session);

            var outcome = session.DeclineDoublingCube(
                request.PlayerId,
                boardState,
                now);

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionNotifier.SessionUpdated(
                session.Id,
                new SessionUpdatedMessage
                {
                    EventType = SessionEventType.DoubleDeclined,
                    Snapshot = _gameSessionSnapshotFactory.Create(session)
                });

            return Unit.Value;
        }
    }
}
