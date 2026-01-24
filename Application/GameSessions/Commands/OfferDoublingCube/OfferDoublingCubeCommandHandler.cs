using Application.GameSessions.Realtime;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.OfferDoublingCube
{
    public class OfferDoublingCubeCommandHandler : IRequestHandler<OfferDoublingCubeCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;

        public OfferDoublingCubeCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(
            OfferDoublingCubeCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;

            var result = session.OfferDoublingCube(
                request.PlayerId,
                now);

            await _uow.CommitAsync();

            await _gameSessionNotifier.DoublingCubeOffered(
                session.Id,
                result.OfferingPlayerId,
                result.OfferedCubeValue);

            return Unit.Value;
        }
    }
}
