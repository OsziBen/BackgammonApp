using Application.GameSessions.Services.SessionCodeGenerator;
using Application.Interfaces;
using Application.Shared.Time;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.CreateGameSession
{
    public class CreateGameSessionCommandHandler : IRequestHandler<CreateGameSessionCommand, Guid>
    {
        private readonly IUnitOfWork _uow;
        private readonly ISessionCodeGenerator _sessionCodeGenerator;
        private readonly IDateTimeProvider _timeProvider;

        public CreateGameSessionCommandHandler(
            IUnitOfWork uow,
            ISessionCodeGenerator sessionCodeGenerator,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _sessionCodeGenerator = sessionCodeGenerator;
            _timeProvider = timeProvider;
        }

        public async Task<Guid> Handle(
            CreateGameSessionCommand request,
            CancellationToken cancellationToken)
        {
            var hasActiveSession = await _uow.GameSessions.HasActiveSession(request.HostPlayerId);

            if (hasActiveSession) {
                throw new InvalidOperationException(    // TODO: custom exception
                    "Player already has an active game session.");
            }

            var sessionCode = _sessionCodeGenerator.Generate();
            var now = _timeProvider.UtcNow;

            var session = GameSessionFactory.Create(
                sessionCode,
                request.Settings,
                now);

            await _uow.GameSessions.AddAsync(session);
            await _uow.CommitAsync();

            return session.Id;
        }
    }
}
