using Application.GameSessions.Commands.StartGameSession;
using Application.GameSessions.Guards;
using Application.GameSessions.Responses;
using Application.Interfaces;
using Application.Shared;
using Common.Enums.BoardState;
using Common.Exceptions;
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

            var player = session.Players
                .FirstOrDefault(p => p.UserId == request.UserId);

            bool isRejoin;

            if (player == null)
            {
                // JOIN
                JoinGameSessionGuards.EnsureSessionNotFull(session);

                player = GamePlayerFactory.Create(
                    session.Id,
                    request.UserId,
                    session.Players.Count == 0);

                await _uow.GamePlayers.AddAsync(player);
                isRejoin = false;
            }
            else
            {
                // REJOIN
                player.IsConnected = true;
                player.LastConnectedAt = DateTimeOffset.UtcNow;
                _uow.GamePlayers.Update(player);
                isRejoin = true;
            }

            session.LastUpdatedAt = DateTimeOffset.UtcNow;

            await _uow.CommitAsync();

            if (!isRejoin && session.Players.Count + 1 == 2)
            {
                await _mediator.Send(
                    new StartGameSessionCommand(session.Id),
                    cancellationToken);
            }

            return new GameSessionSnapshotResponse
            {
                SessionId = session.Id,
                PlayerId = player.Id,
                PlayerColor = player.Color,
                CurrentPhase = session.CurrentPhase,
                CurrentPlayerId = session.CurrentPlayerId,
                BoardStateJson = session.CurrentBoardStateJson,
                IsRejoin = isRejoin
            };
        }
    }
}
