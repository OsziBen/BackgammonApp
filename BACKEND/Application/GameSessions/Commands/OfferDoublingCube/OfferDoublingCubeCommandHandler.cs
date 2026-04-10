using Application.GameSessions.Services.GameSessionBroadcaster;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.GameSession;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.OfferDoublingCube
{
    public class OfferDoublingCubeCommandHandler : IRequestHandler<OfferDoublingCubeCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionBroadcaster _gameSessionBroadcaster;

        public OfferDoublingCubeCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider,
            IGameSessionBroadcaster gameSessionBroadcaster)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _gameSessionBroadcaster = gameSessionBroadcaster;
        }

        public async Task<Unit> Handle(
            OfferDoublingCubeCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var playerId = session.Players
               .FirstOrDefault(p => p.UserId == request.UserId)?.Id
               ?? throw new InvalidOperationException("User is not part of this session");

            var now = _timeProvider.UtcNow;

            var result = session.OfferDoublingCube(
                playerId,
                now);

            session.MarkUpdated(now);
            session.IncrementVersion();

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionBroadcaster.BroadcastAsync(session, SessionEventType.DoubleOffered);

            return Unit.Value;
        }
    }
}
