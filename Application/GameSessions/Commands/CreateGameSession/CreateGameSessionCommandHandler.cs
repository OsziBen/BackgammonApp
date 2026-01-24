using Application.GameSessions.Services.SessionCodeGenerator;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GameSession;
using Application.Shared.Time;
using Common.Enums;
using Common.Exceptions;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.CreateGameSession
{
    public class CreateGameSessionCommandHandler : IRequestHandler<CreateGameSessionCommand, Guid>
    {
        private readonly IUnitOfWork _uow;
        private readonly ISessionCodeGenerator _sessionCodeGenerator;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IGameSessionReadRepository _gameSessionReadRepository;

        public CreateGameSessionCommandHandler(
            IUnitOfWork uow,
            ISessionCodeGenerator sessionCodeGenerator,
            IDateTimeProvider timeProvider,
            IGameSessionReadRepository gameSessionReadRepository)
        {
            _uow = uow;
            _sessionCodeGenerator = sessionCodeGenerator;
            _timeProvider = timeProvider;
            _gameSessionReadRepository = gameSessionReadRepository;
        }

        public async Task<Guid> Handle(
            CreateGameSessionCommand request,
            CancellationToken cancellationToken)
        {
            var hasActiveSession =
                await _gameSessionReadRepository
                    .HasActiveSession(request.HostPlayerId);

            if (hasActiveSession)
            {
                throw new BusinessRuleException(
                    FunctionCode.SessionAlreadyStarted,
                    "Player already has an active game session.");
            }

            var sessionCode = _sessionCodeGenerator.Generate();
            var now = _timeProvider.UtcNow;

            var session = GameSessionFactory.Create(
                sessionCode,
                request.Settings,
                now);

            await _uow.GameSessionsWrite.AddAsync(session);
            await _uow.CommitAsync();

            return session.Id;
        }
    }
}
