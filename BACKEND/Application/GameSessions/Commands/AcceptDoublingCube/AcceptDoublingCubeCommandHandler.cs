using Application.GameSessions.Services.GameSessionBroadcaster;
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
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionBroadcaster _gameSessionBroadcaster;

        public AcceptDoublingCubeCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider,
            IGameSessionBroadcaster gameSessionBroadcaster)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _gameSessionBroadcaster = gameSessionBroadcaster;
        }

        public async Task<Unit> Handle(AcceptDoublingCubeCommand request, CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var playerId = session.Players
               .FirstOrDefault(p => p.UserId == request.UserId)?.Id
               ?? throw new InvalidOperationException("User is not part of this session");

            var now = _timeProvider.UtcNow;

            var result = session.AcceptDoublingCube(
                playerId,
                now);

            session.MarkUpdated(now);
            session.IncrementVersion();

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionBroadcaster.BroadcastAsync(session, SessionEventType.DoubleAccepted);

            return Unit.Value;
        }
    }
}
