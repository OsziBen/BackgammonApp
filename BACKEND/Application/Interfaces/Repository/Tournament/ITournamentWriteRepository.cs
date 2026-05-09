namespace Application.Interfaces.Repository.Tournament
{
    public interface ITournamentWriteRepository
    {
        Task<Domain.Tournament.Tournament?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Domain.Tournament.Tournament tournament, CancellationToken cancellationToken);
    }
}
