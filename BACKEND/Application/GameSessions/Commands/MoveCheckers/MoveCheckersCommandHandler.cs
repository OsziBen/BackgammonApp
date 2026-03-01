using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.GameSessions.Services.GameSessionSnapshotFactory;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Realtime;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameLogic;
using Domain.GameLogic.Generators;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.MoveCheckers
{
    public class MoveCheckersCommandHandler : IRequestHandler<MoveCheckersCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBoardStateFactory _boardStateFactory;
        private readonly IMoveSequenceGenerator _moveSequenceGenerator;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionSnapshotFactory _gameSessionSnapshotFactory;

        public MoveCheckersCommandHandler(
            IUnitOfWork uow,
            IBoardStateFactory boardStateFactory,
            IMoveSequenceGenerator moveSequenceGenerator,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider,
            IGameSessionSnapshotFactory gameSessionSnapshotFactory)
        {
            _uow = uow;
            _boardStateFactory = boardStateFactory;
            _moveSequenceGenerator = moveSequenceGenerator;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
            _gameSessionSnapshotFactory = gameSessionSnapshotFactory;
        }

        public async Task<Unit> Handle(
            MoveCheckersCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var boardState = _boardStateFactory.Create(session);
            var diceRoll = session.GetCurrentDiceRoll();

            var possibleSequences =
                _moveSequenceGenerator.Generate(boardState, diceRoll);

            var clientSequence = new MoveSequence(
                request.Moves
                    .Select(m => new Move(m.From, m.To, m.Die))
                    .ToList()
            );

            if (!possibleSequences.Contains(clientSequence))
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidMove,
                    "Invalid move sequence");
            }

            var now = _timeProvider.UtcNow;

            var nextState = session.ApplyMoveSequence(
                boardState,
                clientSequence,
                request.PlayerId,
                now,
                out var outcome);

            session.UpdateBoardState(
                BoardStateMapper.ToJson(session, nextState));

            session.MarkUpdated(now);

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionNotifier.SessionUpdated(
                session.Id,
                new SessionUpdatedMessage
                {
                    EventType = outcome == null ? SessionEventType.CheckerMoved : SessionEventType.GameFinished,
                    Snapshot = _gameSessionSnapshotFactory.Create(session)
                });

            return Unit.Value;
        }
    }
}
