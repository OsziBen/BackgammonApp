using Application.GameSessions.Commands.StartGameSession;
using Application.GameSessions.Guards;
using Application.GameSessions.Responses;
using Application.Interfaces;
using Application.Shared;
using Common.Enums.GameSession;
using Domain.GamePlayer;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.JoinGameSession
{
    public class JoinGameSessionCommandHandler : IRequestHandler<JoinGameSessionCommand, GameSessionSnapshotResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public JoinGameSessionCommandHandler(
            IUnitOfWork uow,
            IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
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

            var joinResult = session.JoinPlayer(request.UserId);

            if (!joinResult.IsRejoin)
            {
                await _uow.GamePlayers.AddAsync(joinResult.Player);
            }
            else
            {
                _uow.GamePlayers.Update(joinResult.Player);
            }

            session.LastUpdatedAt = DateTimeOffset.UtcNow;

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
