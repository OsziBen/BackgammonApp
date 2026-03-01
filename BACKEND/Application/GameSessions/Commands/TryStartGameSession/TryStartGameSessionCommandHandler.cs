using Application.GameSessions.Commands.StartGameSession;
using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.GameSessions.Services.GameSessionSnapshotFactory;
using Application.Interfaces.Repository;
using Application.Shared;
using Common.Enums.GameSession;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.TryStartGameSession
{
    public class TryStartGameSessionCommandHandler : IRequestHandler<TryStartGameSessionCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IGameSessionSnapshotFactory _gameSessionSnapshotFactory;

        public TryStartGameSessionCommandHandler(
            IUnitOfWork uow,
            IMediator mediator,
            IGameSessionNotifier gameSessionNotifier,
            IGameSessionSnapshotFactory gameSessionSnapshotFactory)
        {
            _uow = uow;
            _mediator = mediator;
            _gameSessionNotifier = gameSessionNotifier;
            _gameSessionSnapshotFactory = gameSessionSnapshotFactory;
        }

        public async Task<Unit> Handle(TryStartGameSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            if (!session.CanStartGame())
            {
                await _gameSessionNotifier.SessionUpdated(
                    session.Id,
                    new SessionUpdatedMessage
                    {
                        EventType = request.IsRejoin
                            ? SessionEventType.PlayerReconnected
                            : SessionEventType.PlayerJoined,
                        Snapshot = _gameSessionSnapshotFactory.Create(session)
                    });

                return Unit.Value;
            }

            await _mediator.Send(
                new StartGameSessionCommand(session.Id),
                cancellationToken);

            return Unit.Value;
        }
    }
}
