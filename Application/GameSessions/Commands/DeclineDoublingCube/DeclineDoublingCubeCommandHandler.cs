using Application.GameSessions.Realtime;
using Application.Interfaces;
using Application.Realtime;
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

        public DeclineDoublingCubeCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider,
            IBoardStateFactory boardStateFactory)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
            _boardStateFactory = boardStateFactory;
        }

        public async Task<Unit> Handle(DeclineDoublingCubeCommand request, CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessions
                .GetByIdAsync(request.SessionId, asNoTracking: false)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;
            var boardState = _boardStateFactory.Create(session);

            var outcome = session.DeclineDoublingCube(
                request.PlayerId,
                boardState,
                now);

            await _uow.CommitAsync();

            await _gameSessionNotifier.GameFinished(
                session.Id,
                session.WinnerPlayerId!.Value,
                GameFinishReason.CubeDeclined,
                outcome);

            return Unit.Value;
        }
    }
}
