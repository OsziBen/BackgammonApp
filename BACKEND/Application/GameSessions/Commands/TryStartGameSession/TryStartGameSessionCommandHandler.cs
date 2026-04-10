using Application.GameSessions.Commands.StartGameSession;
using Application.Interfaces.Repository;
using Application.Shared;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.TryStartGameSession
{
    public class TryStartGameSessionCommandHandler : IRequestHandler<TryStartGameSessionCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public TryStartGameSessionCommandHandler(
            IUnitOfWork uow,
            IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(TryStartGameSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite
                .GetByIdAsync(request.SessionId, cancellationToken)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            if (!session.CanStartGame())
            {
                return Unit.Value;
            }

            await _mediator.Send(
                new StartGameSessionCommand(session.Id),
                cancellationToken);

            return Unit.Value;
        }
    }
}
