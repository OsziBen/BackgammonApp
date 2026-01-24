using Application.GameSessions.Realtime;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.GameSession;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.PlayerForfeit
{
    public class PlayerForfeitCommandHandler : IRequestHandler<PlayerForfeitCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IBoardStateFactory _boardStateFactory;

        public PlayerForfeitCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider,
            IBoardStateFactory boardStateFactory)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
            _boardStateFactory = boardStateFactory;
        }

        public async Task<Unit> Handle(
            PlayerForfeitCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;
            var boardState = _boardStateFactory.Create(session);

            var resultType = session.Forfeit(
                request.PlayerId,
                boardState,
                now);

            await _uow.CommitAsync();

            await _gameSessionNotifier.GameFinished(
                session.Id,
                session.WinnerPlayerId!.Value,
                GameFinishReason.Forfeit,
                resultType);

            return Unit.Value;
        }
    }
}
