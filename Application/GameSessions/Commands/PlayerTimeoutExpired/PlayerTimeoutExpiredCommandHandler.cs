using Application.GameSessions.Realtime;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GamePlayer;
using Application.Shared;
using Application.Shared.Time;
using Domain.GamePlayer;
using Domain.GameSession;
using MediatR;


namespace Application.GameSessions.Commands.PlayerTimeoutExpired
{
    public class PlayerTimeoutExpiredCommandHandler : IRequestHandler<PlayerTimeoutExpiredCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGamePlayerReadRepository _playerReadRepo;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IDateTimeProvider _timeProvider;

        public PlayerTimeoutExpiredCommandHandler(
            IUnitOfWork uow,
            IGamePlayerReadRepository playerReadRepo,
            IGameSessionNotifier gameSessionNotifier,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _playerReadRepo = playerReadRepo;
            _gameSessionNotifier = gameSessionNotifier;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(
            PlayerTimeoutExpiredCommand request,
            CancellationToken cancellationToken)
        {
            var now = _timeProvider.UtcNow;

            var player = await _playerReadRepo
                .GetByIdAsync(request.GamePlayerId)
                .GetOrThrowAsync(nameof(GamePlayer), request.GamePlayerId);

            if (player.IsConnected)
            {
                return Unit.Value;
            }

            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(player.GameSessionId)
                .GetOrThrowAsync(nameof(GameSession), player.GameSessionId);

            if (session.IsFinished)
            {
                return Unit.Value;
            }

            var opponent = await _playerReadRepo
                .GetOpponentAsync(session.Id, player.Id)
                .GetOrThrowAsync(nameof(GameSession), player.GameSessionId);

            session.Finish(opponent?.Id ?? Guid.Empty, now);

            await _uow.CommitAsync();

            await _gameSessionNotifier.PlayerTimeoutExpired(
                session.Id,
                player.Id,
                session.WinnerPlayerId);

            return Unit.Value;
        }
    }
}
