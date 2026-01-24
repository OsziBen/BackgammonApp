namespace Application.Interfaces.Repository.GamePlayer
{
    public interface IGamePlayerReadRepository
    {
        Task<Domain.GamePlayer.GamePlayer?> GetBySessionAndUserAsync(
            Guid sessionId,
            Guid userId);

        Task<Domain.GamePlayer.GamePlayer?> GetByIdAsync(
            Guid id);

        Task<Domain.GamePlayer.GamePlayer?> GetOpponentAsync(
            Guid sessionId,
            Guid excludePlayerId);

        Task<IReadOnlyList<Domain.GamePlayer.GamePlayer>> GetPlayersBySessionAsync(
            Guid sessionId);

        Task<IReadOnlyList<Domain.GamePlayer.GamePlayer>> GetExpiredPlayersAsync(
            DateTimeOffset now,
            TimeSpan disconnectTimeout);
    }
}
