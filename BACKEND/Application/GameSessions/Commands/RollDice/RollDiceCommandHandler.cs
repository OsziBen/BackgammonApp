using Application.GameSessions.Services.GameSessionBroadcaster;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.GameSession;
using Domain.GameLogic;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.RollDice
{
    public class RollDiceCommandHandler : IRequestHandler<RollDiceCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IDiceRoller _diceRoller;
        private readonly IGameSessionBroadcaster _gameSessionBroadcaster;

        public RollDiceCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider,
            IDiceRoller diceRoller,
            IGameSessionBroadcaster gameSessionBroadcaster)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _diceRoller = diceRoller;
            _gameSessionBroadcaster = gameSessionBroadcaster;
        }

        public async Task<Unit> Handle(
            RollDiceCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var playerId = session.Players
                .FirstOrDefault(p => p.UserId == request.UserId)?.Id
                ?? throw new InvalidOperationException("User is not part of this session");

            var now = _timeProvider.UtcNow;

            var diceRoll = session.RollDice(
                playerId,
                _diceRoller,
                now);

            session.MarkUpdated(now);
            session.IncrementVersion();

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionBroadcaster.BroadcastAsync(session, SessionEventType.DiceRolled);

            return Unit.Value;
        }
    }
}
