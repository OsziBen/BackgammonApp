namespace Application.Interfaces.Repository.GamePlayer
{
    public interface IGamePlayerWriteRepository
    {
        Task<Domain.GamePlayer.GamePlayer?> GetBySessionAndUserAsync(Guid sessionId, Guid userId);
        Task<Domain.GamePlayer.GamePlayer?> GetByIdAsync(Guid id);
        Task AddAsync(Domain.GamePlayer.GamePlayer player);
        void Update(Domain.GamePlayer.GamePlayer player);
        void Remove(Domain.GamePlayer.GamePlayer player);
    }
}
