using Application.GameSessions.Commands.StartGameSession;
using Application.GameSessions.Responses;
using Application.Interfaces;
using Application.Shared;
using Application.Shared.Time;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.JoinGameSession
{
    public class JoinGameSessionCommandHandler : IRequestHandler<JoinGameSessionCommand, GameSessionSnapshotResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        private readonly IDateTimeProvider _timeProvider;

        public JoinGameSessionCommandHandler(
            IUnitOfWork uow,
            IMediator mediator,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _mediator = mediator;
            _timeProvider = timeProvider;
        }

        public async Task<GameSessionSnapshotResponse> Handle(
            JoinGameSessionCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessions
                .GetBySessionCodeAsync(
                    request.SessionCode,
                    includePlayers: true,
                    asNoTracking: false)
                .GetOrThrowAsync(nameof(GameSession), request.SessionCode);

            var now = _timeProvider.UtcNow;

            var joinResult = session.JoinPlayer(request.UserId, now);

            if (!joinResult.IsRejoin)
            {
                await _uow.GamePlayers.AddAsync(joinResult.Player);
            }
            else
            {
                _uow.GamePlayers.Update(joinResult.Player);
            }

            session.LastUpdatedAt = now;

            await _uow.CommitAsync();

            if (!joinResult.IsRejoin && session.CanStartGame())
            {
                await _mediator.Send(
                    new StartGameSessionCommand(session.Id),
                    cancellationToken);
            }

            return new GameSessionSnapshotResponse
            {
                SessionId = session.Id,
                PlayerId = joinResult.Player.Id,
                PlayerColor = joinResult.Player.Color,
                CurrentPhase = session.CurrentPhase,
                CurrentPlayerId = session.CurrentPlayerId,
                BoardStateJson = session.CurrentBoardStateJson,
                IsRejoin = joinResult.IsRejoin,
            };
        }
    }
}
