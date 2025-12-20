using Application.GameSessions.Realtime;
using Application.Interfaces;
using MediatR;


namespace Application.GameSessions.Commands.PlayerTimeoutExpired
{
    public class PlayerTimeoutExpiredCommandHandler : IRequestHandler<PlayerTimeoutExpiredCommand>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;

        public PlayerTimeoutExpiredCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
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

            session.IsFinished = true;
            session.FinishedAt = DateTimeOffset.UtcNow;

            var opponent = await _uow.GamePlayers.GetOpponentAsync(session.Id, player.Id, asNoTracking: false);

            if (opponent != null)
            {
                session.WinnerPlayerId = opponent.Id;
            }

            session.LastUpdatedAt = DateTimeOffset.UtcNow;

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
