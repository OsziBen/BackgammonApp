namespace Application.Interfaces.Repository.GameSession
{
    public interface IGameSessionWriteRepository
    {
        Task<Domain.GameSession.GameSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Domain.GameSession.GameSession session, CancellationToken cancellationToken);
        Task<Domain.GameSession.GameSession?> GetBySessionCodeAsync(string sessionCode, CancellationToken cancellationToken, bool includePlayers = false);
    }
}
