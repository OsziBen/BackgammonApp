using Application.GameSessions.Realtime;
using Application.Interfaces;
using Application.Shared.Time;
using MediatR;


namespace Application.GameSessions.Commands.PlayerTimeoutExpired
{
    public class PlayerTimeoutExpiredCommandHandler : IRequestHandler<PlayerTimeoutExpiredCommand>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;

        public PlayerTimeoutExpiredCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
        }

        public async Task Handle(
            PlayerTimeoutExpiredCommand request,
            CancellationToken cancellationToken)
        {
            var player = await _uow.GamePlayers
                .GetByIdAsync(request.GamePlayerId, asNoTracking: false);

            if (player == null || player.IsConnected)
            {
                return;
            }

            var session = await _uow.GameSessions.GetByIdAsync(
                player.GameSessionId,
                asNoTracking: false);

            if (session == null || session.IsFinished)
            {
                return;
            }
            
            var now = _timeProvider.UtcNow;

            session.IsFinished = true;
            session.FinishedAt = now;

            var opponent = await _uow.GamePlayers.GetOpponentAsync(session.Id, player.Id, asNoTracking: false);

            if (opponent != null)
            {
                session.WinnerPlayerId = opponent.Id;
            }

            session.LastUpdatedAt = now;

            // TODO:
            // player.Forfeit();
            // session.End();

            await _uow.CommitAsync();

            await _gameSessionNotifier.PlayerTimeoutExpired(
                session.Id,
                player.Id,
                session.WinnerPlayerId);
        }
    }
}
