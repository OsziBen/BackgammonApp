using Application.Interfaces;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.CreateGameSession
{
    public class CreateGameSessionCommandHandler : IRequestHandler<CreateGameSessionCommand, Guid>
    {
        private readonly IUnitOfWork _uow;

        public CreateGameSessionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid> Handle(
            CreateGameSessionCommand request,
            CancellationToken cancellationToken)
        {
            var session = GameSessionFactory.Create(
                request.MatchId,
                request.SessionCode);

            await _uow.GameSessions.AddAsync(session);
            await _uow.CommitAsync();

            return session.Id;
        }
    }
}
