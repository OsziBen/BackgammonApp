using Application.GameSessions.Guards;
using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.Interfaces;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.GameSession;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.RollDice
{
    public class RollDiceCommandHandler : IRequestHandler<RollDiceCommand, RollDiceResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDiceService _diceService;
        private readonly IDateTimeProvider _timeProvider;

        public RollDiceCommandHandler(
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

        public async Task<RollDiceResult> Handle(
            RollDiceCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessions
                .GetByIdAsync(request.SessionId, asNoTracking: false)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;

            GameSessionGuards.EnsureNotFinished(session);
            GamePhaseGuards.EnsurePhase(
                session,
                GamePhase.RollDice);
            GameSessionStateGuards.EnsureNoActiveDice(session);
            RollDiceGuards.EnsureCurrentPlayer(
                session,
                request.PlayerId);

            // DOBÁS
            var die1 = _diceService.Roll();
            var die2 = _diceService.Roll();

            var movesCount = die1 == die2 ? 4 : 2;

            session.LastDiceRoll = new[] { die1, die2 };
            session.CurrentPhase = GamePhase.MoveCheckers;
            session.LastUpdatedAt = now;

            //_uow.GameSessions.Update(session);
            await _uow.CommitAsync();

            await _gameSessionNotifier.DiceRolled(
                session.Id,
                request.PlayerId,
                die1,
                die2);

            return new RollDiceResult(
                die1,
                die2,
                movesCount);
        }
    }
}
