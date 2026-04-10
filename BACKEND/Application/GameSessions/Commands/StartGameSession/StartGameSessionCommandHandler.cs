using Application.GameSessions.Services.GameSessionBroadcaster;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Realtime;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.GameSession;
using Domain.GameSession;
using Domain.GameSession.Services;
using MediatR;

namespace Application.GameSessions.Commands.StartGameSession
{
    public class StartGameSessionCommandHandler : IRequestHandler<StartGameSessionCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IStartingPlayerRoller _startingPlayerRoller;
        private readonly IBoardStateFactory _boardStateFactory;
        private readonly IGameSessionBroadcaster _gameSessionBroadcaster;

        public StartGameSessionCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider,
            IStartingPlayerRoller startingPlayerRoller,
            IBoardStateFactory boardStateFactory,
            IGameSessionBroadcaster gameSessionBroadcaster)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _startingPlayerRoller = startingPlayerRoller;
            _boardStateFactory = boardStateFactory;
            _gameSessionBroadcaster = gameSessionBroadcaster;
        }

        public async Task<Unit> Handle(
            StartGameSessionCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            if (!session.CanStartGame())
            {
                return Unit.Value;
            }

            var now = _timeProvider.UtcNow;

            session.Start(now);

            session.DetermineStartingPlayer(_startingPlayerRoller, now);

            var initialBoard = _boardStateFactory.CreateInitial(session);

            session.UpdateBoardState(
                BoardStateMapper.ToJson(session, initialBoard));

            session.MarkUpdated(now);
            session.IncrementVersion();

            await _uow.CommitAsync(cancellationToken);

            await _gameSessionBroadcaster.BroadcastAsync(session, SessionEventType.GameStarted);

            return Unit.Value;
        }
    }
}
