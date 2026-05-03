namespace Application.Interfaces.Repository.Tournament
{
    public interface ITournamentWriteRepository
    {
        Task AddAsync(Domain.Tournament.Tournament tournament, CancellationToken cancellationToken);
    }
}
