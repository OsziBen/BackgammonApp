namespace Application.Interfaces.Repository.GameSession
{
    public interface IGameSessionWriteRepository
    {
        Task<Domain.GameSession.GameSession?> GetByIdAsync(Guid id);
        Task AddAsync(Domain.GameSession.GameSession session);
        void Update(Domain.GameSession.GameSession session);
        Task<Domain.GameSession.GameSession?> GetBySessionCodeAsync(string sessionCode, bool includePlayers = false);
    }
}
