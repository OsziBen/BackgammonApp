using Application.GameSessions.Realtime;
using Application.Interfaces;
using Application.Shared;
using Application.Shared.Time;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.AcceptDoublingCube
{
    public class AcceptDoublingCubeCommandHandler : IRequestHandler<AcceptDoublingCubeCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;

        public AcceptDoublingCubeCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(AcceptDoublingCubeCommand request, CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessions
                .GetByIdAsync(request.SessionId, asNoTracking: false)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;

            var result = session.AcceptDoublingCube(
                request.PlayerId,
                now);

            await _uow.CommitAsync();

            await _gameSessionNotifier.DoublingCubeAccepted(
                session.Id,
                result.AcceptingPlayerId,
                result.NewCubeValue);

            return Unit.Value;
        }
    }
}
