namespace Application.Interfaces.Repository.GameSession
{
    public interface IGameSessionReadRepository
    {
        Task<Domain.GameSession.GameSession?> GetByIdAsync(Guid id);
        Task<Domain.GameSession.GameSession?> GetBySessionCodeAsync(string sessionCode);
        Task<bool> HasActiveSession(Guid playerId);
    }
}
