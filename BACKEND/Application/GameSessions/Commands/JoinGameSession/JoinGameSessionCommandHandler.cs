using Application.GameSessions.Commands.TryStartGameSession;
using Application.GameSessions.Services.GameSessionBroadcaster;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GameSession;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.GameSession;
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
        private readonly IGameSessionBroadcaster _gameSessionBroadcaster;
        private readonly IMediator _mediator;

        public JoinGameSessionCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider,
            IGameSessionReadRepository gameSessionReadRepository,
            IGameSessionBroadcaster gameSessionBroadcaster,
            IMediator mediator)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _gameSessionReadRepository = gameSessionReadRepository;
            _gameSessionBroadcaster = gameSessionBroadcaster;
            _mediator = mediator;
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
            session.IncrementVersion();

            await _uow.CommitAsync(cancellationToken);

            var eventType = joinResult.IsRejoin ? SessionEventType.PlayerReconnected : SessionEventType.PlayerJoined;

            var freshSession = await _gameSessionReadRepository
                .GetByIdWithPlayersAndUsersAsync(session.Id, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), session.Id);

            await _gameSessionBroadcaster.BroadcastAsync(freshSession, eventType, isRejoin: joinResult.IsRejoin);

            await _mediator.Send(new TryStartGameSessionCommand(freshSession.Id), cancellationToken);

            return joinResult;
        }
    }
}
