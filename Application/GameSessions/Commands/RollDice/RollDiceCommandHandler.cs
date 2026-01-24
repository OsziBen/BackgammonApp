using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Domain.GameLogic;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.RollDice
{
    public class RollDiceCommandHandler : IRequestHandler<RollDiceCommand, RollDiceResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IDiceRoller _diceRoller;

        public RollDiceCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider,
            IDiceRoller diceRoller)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
            _diceRoller = diceRoller;
        }

        public async Task<RollDiceResult> Handle(
            RollDiceCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;

            var diceRoll = session.RollDice(
                request.PlayerId,
                _diceRoller,
                now);

            await _uow.CommitAsync();

            await _gameSessionNotifier.DiceRolled(
                session.Id,
                request.PlayerId,
                diceRoll.Values[0],
                diceRoll.IsDouble ? diceRoll.Values[0] : diceRoll.Values[1]);

            return new RollDiceResult(
                diceRoll.Values[0],
                diceRoll.IsDouble ? diceRoll.Values[0] : diceRoll.Values[1],
                diceRoll.Values.Count);
        }
    }
}
