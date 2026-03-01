using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.GameSessions.Services.GameSessionSnapshotFactory;
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
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IDiceRoller _diceRoller;
        private readonly IGameSessionSnapshotFactory _gameSessionSnapshotFactory;

        public RollDiceCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider,
            IDiceRoller diceRoller,
            IGameSessionSnapshotFactory gameSessionSnapshotFactory)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
            _diceRoller = diceRoller;
            _gameSessionSnapshotFactory = gameSessionSnapshotFactory;
        }

        public async Task<Unit> Handle(
            RollDiceCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;

            var diceRoll = session.RollDice(
                request.PlayerId,
                _diceRoller,
                now);

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionNotifier.SessionUpdated(
                session.Id,
                new SessionUpdatedMessage
                {
                    EventType = SessionEventType.DiceRolled,
                    Snapshot = _gameSessionSnapshotFactory.Create(session)
                });

            return Unit.Value;
        }
    }
}
