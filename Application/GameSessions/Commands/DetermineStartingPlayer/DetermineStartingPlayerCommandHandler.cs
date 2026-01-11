using Application.GameSessions.Realtime;
using Application.Interfaces;
using Application.Shared;
using Application.Shared.Time;
using Common.Constants;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.DetermineStartingPlayer
{
    public class DetermineStartingPlayerCommandHandler : IRequestHandler<DetermineStartingPlayerCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDiceService _diceService;
        private readonly IDateTimeProvider _timeProvider;

        public DetermineStartingPlayerCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDiceService diceService,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _diceService = diceService;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(
            DetermineStartingPlayerCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessions
                .GetByIdAsync(request.SessionId, asNoTracking: false)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;

            var players = await _uow.GamePlayers
                .GetPlayersBySessionAsync(session.Id, asNoTracking: false);

            if (players.Count < GameSessionConstants.MaxPlayers)
            {
                return Unit.Value;
            }

            var (roll1, roll2) = _diceService.RollDistinctPair();

            var result = session.DetermineStartingPlayer(
                players,
                roll1,
                roll2,
                now);

            await _uow.CommitAsync();

            await _gameSessionNotifier.StartingPlayerDetermined(
                session.Id,
                result.Rolls.ToArray(),
                result.StarttingPlayerId);

            return Unit.Value;
        }
    }
}
