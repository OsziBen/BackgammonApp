using Application.GameSessions.Responses;
using Application.Interfaces;
using Common.Enums;
using Common.Enums.BoardState;
using Common.Exceptions;
using Domain.GamePlayer;
using MediatR;

namespace Application.GameSessions.Commands.JoinGameSession
{
    public class JoinGameSessionCommandHandler : IRequestHandler<JoinGameSessionCommand, GameSessionSnapshotResponse>
    {
        private readonly IUnitOfWork _uow;

        public JoinGameSessionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GameSessionSnapshotResponse> Handle(
            JoinGameSessionCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessions
                .GetBySessionCodeAsync(
                request.SessionCode,
                includePlayers: true,
                asNoTracking: false);

            if (session == null)
            {
                throw new NotFoundException(
                    FunctionCode.ResourceNotFound,
                    $"Game session with code '{request.SessionCode}' was not found.");
            }

            var player = session.Players
                .FirstOrDefault(p => p.UserId == request.UserId);

            bool isRejoin;

            if (player == null)
            {
                // JOIN
                if (session.Players.Count >= 2)
                {
                    throw new BusinessRuleException(FunctionCode.SessionFull, "Session is full");
                }

                player = new GamePlayer
                {
                    Id = Guid.NewGuid(),
                    GameSessionId = session.Id,
                    UserId = request.UserId,
                    IsHost = session.Players.Count == 0,
                    Color = session.Players.Count == 0
                        ? PlayerColor.White
                        : PlayerColor.Black,
                    IsConnected = true,
                    LastConnectedAt = DateTimeOffset.UtcNow
                };

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
            _uow.GameSessions.Update(session);

            await _uow.CommitAsync();

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
