using Application.GameSessions.Realtime;
using Application.Interfaces;
using Application.Shared;
using Application.Shared.Time;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.DetermineStartingPlayer
{
    public class DetermineStartingPlayerCommandHandler : IRequestHandler<DetermineStartingPlayerCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IStartingPlayerRoller _startingPlayerRoller;

        public DetermineStartingPlayerCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider,
            IStartingPlayerRoller startingPlayerRoller)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
            _startingPlayerRoller = startingPlayerRoller;
        }

        public async Task<Unit> Handle(
            DetermineStartingPlayerCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessions
                .GetByIdAsync(request.SessionId, asNoTracking: false)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;

            var result = session.DetermineStartingPlayer(
                _startingPlayerRoller,
                now);

            await _uow.CommitAsync();

            await _gameSessionNotifier.StartingPlayerDetermined(
                session.Id,
                result.Rolls.ToArray(),
                result.StartingPlayerId);

            return Unit.Value;
        }
    }
}
