using Application.GameSessions.Commands.DetermineStartingPlayer;
using Application.GameSessions.Guards;
using Application.Interfaces;
using Application.Shared;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.StartGameSession
{
    public class StartGameSessionCommandHandler : IRequestHandler<StartGameSessionCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public StartGameSessionCommandHandler(
            IUnitOfWork uow,
            IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(
            StartGameSessionCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessions
                .GetByIdAsync(request.SessionId, asNoTracking: false)
                .GetOrThrowAsync(nameof(GameSession), request.SessionId);

            session.Start();

            await _uow.CommitAsync();

            await _mediator.Send(
                new DetermineStartingPlayerCommand(session.Id),
                cancellationToken);

            return Unit.Value;
        }
    }
}
