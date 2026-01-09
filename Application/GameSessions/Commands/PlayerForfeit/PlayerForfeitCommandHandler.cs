using Application.GameSessions.Realtime;
using Application.Interfaces;
using Application.Shared;
using Common.Enums.GameSession;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.PlayerForfeit
{
    public class PlayerForfeitCommandHandler : IRequestHandler<PlayerForfeitCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGameSessionNotifier _gameSessionNotifier;

        public PlayerForfeitCommandHandler(
            IUnitOfWork uow,
            IGameSessionNotifier gameSessionNotifier)
        {
            _uow = uow;
            _gameSessionNotifier = gameSessionNotifier;
        }

        public async Task<Unit> Handle( // TODO: guard-ok hozzáadása
            PlayerForfeitCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessions
                .GetByIdAsync(
                request.SessionId,
                asNoTracking: false)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            if (session.IsFinished)
            {
                throw new InvalidOperationException("Game already finished");
            }

            if (session.Players.All(p => p.Id != request.PlayerId))
            {
                throw new InvalidOperationException("Player not part of this session");
            }

            session.Forfeit(request.PlayerId);

            await _uow.CommitAsync();

            await _gameSessionNotifier.GameFinished(
                session.Id,
                session.WinnerPlayerId!.Value,
                GameFinishReason.Forfeit);

            return Unit.Value;
        }
    }
}
