namespace Application.Interfaces.Repository.GamePlayer
{
    public interface IGamePlayerWriteRepository
    {
        Task<Domain.GamePlayer.GamePlayer?> GetBySessionAndUserAsync(Guid sessionId, Guid userId);
        Task<Domain.GamePlayer.GamePlayer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Domain.GamePlayer.GamePlayer player, CancellationToken cancellationToken);
        void Update(Domain.GamePlayer.GamePlayer player);
        void Remove(Domain.GamePlayer.GamePlayer player);
    }
}
