using Application.GameSessions.Services.GameSessionBroadcaster;
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
        private readonly IDateTimeProvider _timeProvider;
        private readonly IBoardStateFactory _boardStateFactory;
        private readonly IGameSessionBroadcaster _gameSessionBroadcaster;

        public DeclineDoublingCubeCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider,
            IBoardStateFactory boardStateFactory,
            IGameSessionBroadcaster gameSessionBroadcaster)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _boardStateFactory = boardStateFactory;
            _gameSessionBroadcaster = gameSessionBroadcaster;
        }

        public async Task<Unit> Handle(DeclineDoublingCubeCommand request, CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var playerId = session.Players
               .FirstOrDefault(p => p.UserId == request.UserId)?.Id
               ?? throw new InvalidOperationException("User is not part of this session");

            var now = _timeProvider.UtcNow;
            var boardState = _boardStateFactory.Create(session);

            var outcome = session.DeclineDoublingCube(
                playerId,
                boardState,
                now);

            session.MarkUpdated(now);
            session.IncrementVersion();

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionBroadcaster.BroadcastAsync(session, SessionEventType.DoubleDeclined);

            return Unit.Value;
        }
    }
}
