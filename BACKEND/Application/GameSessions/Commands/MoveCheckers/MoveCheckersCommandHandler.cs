using Application.GameSessions.Services.GameSessionBroadcaster;
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
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionBroadcaster _gameSessionBroadcaster;

        public MoveCheckersCommandHandler(
            IUnitOfWork uow,
            IBoardStateFactory boardStateFactory,
            IMoveSequenceGenerator moveSequenceGenerator,
            IDateTimeProvider timeProvider,
            IGameSessionBroadcaster gameSessionBroadcaster)
        {
            _uow = uow;
            _boardStateFactory = boardStateFactory;
            _moveSequenceGenerator = moveSequenceGenerator;
            _timeProvider = timeProvider;
            _gameSessionBroadcaster = gameSessionBroadcaster;
        }

        public async Task<Unit> Handle(
            MoveCheckersCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var playerId = session.Players
                .FirstOrDefault(p => p.UserId == request.UserId)?.Id
                ?? throw new InvalidOperationException("User is not part of this session");

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
                playerId,
                now,
                out var outcome);

            session.UpdateBoardState(
                BoardStateMapper.ToJson(session, nextState));

            session.MarkUpdated(now);
            session.IncrementVersion();

            await _uow.CommitAsync(cancellationToken);

            var eventType = outcome == null ? SessionEventType.CheckerMoved : SessionEventType.GameFinished;

            await _gameSessionBroadcaster.BroadcastAsync(session, eventType);

            return Unit.Value;
        }
    }
}
