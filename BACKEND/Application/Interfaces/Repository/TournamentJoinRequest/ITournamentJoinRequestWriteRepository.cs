namespace Application.Interfaces.Repository.TournamentJoinRequest
{
    public interface ITournamentJoinRequestWriteRepository
    {
        Task AddAsync(Domain.TournamentJoinRequest.TournamentJoinRequest tournamentJoinRequest, CancellationToken cancellationToken);
        Task<Domain.TournamentJoinRequest.TournamentJoinRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
