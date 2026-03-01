namespace Application.Interfaces.Repository.GameSession
{
    public interface IGameSessionReadRepository
    {
        Task<Domain.GameSession.GameSession?> GetByIdAsync(Guid id);
        Task<Domain.GameSession.GameSession?> GetBySessionCodeAsync(string sessionCode, CancellationToken cancellationToken);
        Task<bool> HasActiveSession(Guid playerId, CancellationToken cancellationToken);
        Task<Domain.GameSession.GameSession?> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> ExistsBySessionCodeAsync(string sessionCode, CancellationToken cancellationToken);
    }
}
