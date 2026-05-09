namespace Application.Interfaces.Repository.Tournament
{
    public interface ITournamentReadRepository
    {
        Task<bool> ExistsByNameAsync(string tournamentName, CancellationToken cancellationToken);
        Task<List<Domain.Tournament.Tournament>> GetAllPublicAsync(CancellationToken cancellationToken);
        Task<Domain.Tournament.Tournament?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Domain.Tournament.Tournament>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
