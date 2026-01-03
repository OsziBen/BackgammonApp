using Application.GameSessions.Guards;
using Application.GameSessions.Realtime;
using Application.Interfaces;
using Application.Realtime;
using Application.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameLogic;
using Domain.GameLogic.Extensions;
using Domain.GameLogic.Generators;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.MoveCheckers
{
    public class MoveCheckersCommandHandler : IRequestHandler<MoveCheckersCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBoardStateFactory _boardStateFactory;
        private readonly IMoveSequenceGenerator _moveSequenceGenerator;
        private readonly IGameSessionNotifier _gameSessionNotifier;

        public MoveCheckersCommandHandler(
            IUnitOfWork uow,
            IBoardStateFactory boardStateFactory,
            IMoveSequenceGenerator moveSequenceGenerator,
            IGameSessionNotifier gameSessionNotifier)
        {
            _uow = uow;
            _boardStateFactory = boardStateFactory;
            _moveSequenceGenerator = moveSequenceGenerator;
            _gameSessionNotifier = gameSessionNotifier;
        }

        public async Task<Unit> Handle(
            MoveCheckersCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessions
                .GetByIdAsync(request.SessionId, asNoTracking: false)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            GameSessionGuards.EnsureNotFinished(session);
            GamePhaseGuards.EnsurePhase(session, GamePhase.MoveCheckers);
            RollDiceGuards.EnsureCurrentPlayer(session, request.PlayerId);
            GameSessionStateGuards.EnsureDiceStateValid(session);
            MoveCheckersGuards.EnsureDiceRolled(session);
            MoveCheckersGuards.EnsureMovesProvided(request.Moves);

            var boardState = _boardStateFactory.Create(session);
            var diceRoll = new DiceRoll(session.LastDiceRoll!);

            var possibleSequences =
                _moveSequenceGenerator.Generate(boardState, diceRoll);

            var clientSequence = new MoveSequence(
                request.Moves
                    .Select(m => m.ToDomain())
                    .ToList());

            if (!possibleSequences.Contains(clientSequence))
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidMove,
                    "Invalid move sequence");
            }

            var nextState = boardState;

            foreach (var move in clientSequence.Moves)
            {
                nextState = nextState.Apply(move);
            }

            if (nextState.IsGameOver(out var winner, out var resultType))
            {
                session.IsFinished = true;
                session.FinishedAt = DateTimeOffset.UtcNow;
                session.CurrentPhase = GamePhase.GameFinished;

                session.WinnerPlayerId = session.Players
                    .Single(p => p.Color == winner)
                    .Id;
            }
            else
            {
                session.EndTurn();
            }

            var json = BoardStateMapper.ToJson(nextState);
            session.UpdateBoardStateJson(json);
            session.LastUpdatedAt = DateTimeOffset.UtcNow;

            await _uow.CommitAsync();

            await _gameSessionNotifier.CheckersMoved(
                session.Id,
                request.PlayerId,
                request.Moves);

            return Unit.Value;
        }
    }
}
