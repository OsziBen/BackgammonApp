using Application.GameSessions.Guards;
using Application.GameSessions.Realtime;
using Application.Interfaces;
using Application.Shared;
using Application.Shared.Time;
using Common.Constants;
using Common.Enums.GameSession;
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

            GameSessionGuards.EnsureNotFinished(session);
            GamePhaseGuards.EnsurePhase(
                session,
                GamePhase.DeterminingStartingPlayer);

            var players = await _uow.GamePlayers
                .GetPlayersBySessionAsync(session.Id, asNoTracking: false);

            GameSessionGuards.EnsureMaxPlayerCount(
                players.Count,
                GameSessionConstants.MaxPlayers);

            if (players.Count < GameSessionConstants.MaxPlayers)
            {
                return Unit.Value;
            }

            var (roll1, roll2) = _diceService.RollDistinctPair();

            players[0].StartingRoll = roll1;
            players[1].StartingRoll = roll2;

            var startingPlayer = roll1 > roll2
                ? players[0]
                : players[1];

            session.CurrentPlayerId = startingPlayer.Id;
            session.CurrentPhase = GamePhase.RollDice;
            session.LastUpdatedAt = now;

            //_uow.GameSessions.Update(session);

            await _uow.CommitAsync();

            await _gameSessionNotifier.StartingPlayerDetermined(
                session.Id,
                new[]
                {
                    (players[0].Id, roll1),
                    (players[1].Id, roll2)
                },
                startingPlayer.Id);

            return Unit.Value;
        }
    }
}
