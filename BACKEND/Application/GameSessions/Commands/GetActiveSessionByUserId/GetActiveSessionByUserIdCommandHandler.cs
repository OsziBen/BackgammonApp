using Application.GameSessions.Responses;
using Application.Interfaces.Repository.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.GetActiveSessionByUserId
{
    public class GetActiveSessionByUserIdCommandHandler : IRequestHandler<GetActiveSessionByUserIdCommand, GetActiveSessionByUserIdResponse?>
    {
        private readonly IGameSessionReadRepository _gameSessionReadRepository;

        public GetActiveSessionByUserIdCommandHandler(IGameSessionReadRepository gameSessionReadRepository)
        {
            _gameSessionReadRepository = gameSessionReadRepository;
        }

        public async Task<GetActiveSessionByUserIdResponse?> Handle(GetActiveSessionByUserIdCommand request, CancellationToken cancellationToken)
        {
            var session = await _gameSessionReadRepository.GetActiveByUserIdAsync(request.UserId, cancellationToken);

            return session == null ? null : new GetActiveSessionByUserIdResponse
            {
                SessionId = session.Id,
                SessionCode = session.SessionCode,
                Settings = session.Settings,
                CreatedAt = session.CreatedAt
            };
        }
    }
}
