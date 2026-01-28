using Application.GameSessions.Commands.DetermineStartingPlayer;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.StartGameSession
{
    public class StartGameSessionCommandHandler : IRequestHandler<StartGameSessionCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        private readonly IDateTimeProvider _timeProvider;

        public StartGameSessionCommandHandler(
            IUnitOfWork uow,
            IMediator mediator,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _mediator = mediator;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(
            StartGameSessionCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            var now = _timeProvider.UtcNow;

            session.Start(now);

            await _uow.CommitAsync();

            await _mediator.Send(
                new DetermineStartingPlayerCommand(session.Id),
                cancellationToken);

            return Unit.Value;
        }
    }
}
