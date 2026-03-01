using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GameSession;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Exceptions;
using Domain.GameSession;
using Domain.GameSession.Results;
using MediatR;

namespace Application.GameSessions.Commands.JoinGameSession
{
    public class JoinGameSessionCommandHandler : IRequestHandler<JoinGameSessionCommand, JoinResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionReadRepository _gameSessionReadRepository;
        public JoinGameSessionCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider,
            IGameSessionReadRepository gameSessionReadRepository)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _gameSessionReadRepository = gameSessionReadRepository;
        }

        public async Task<JoinResult> Handle(
            JoinGameSessionCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetBySessionCodeAsync(request.SessionCode, cancellationToken, includePlayers: true)
                .GetOrThrowAsync(nameof(GameSession), request.SessionCode);

            var now = _timeProvider.UtcNow;

            var activeSession = await _gameSessionReadRepository
                .GetActiveByUserIdAsync(request.UserId, cancellationToken);

            if (activeSession != null && activeSession.Id != session.Id)
            {
                throw new BusinessRuleException(
                    FunctionCode.UserAlreadyInActiveSession,
                    "User already participates in another active session.");
            }

            var joinResult = session.JoinPlayer(request.UserId, now);

            if (!joinResult.IsRejoin)
            {
                await _uow.GamePlayersWrite.AddAsync(joinResult.Player, cancellationToken);
            }

            session.MarkUpdated(now);

            await _uow.CommitAsync(cancellationToken);

            return joinResult;
        }
    }
}
