namespace Application.Interfaces.Repository.TournamentParticipant
{
    public interface ITournamentParticipantWriteRepository
    {
        Task<Domain.TournamentParticipant.TournamentParticipant?> GetAsync(Guid userId, Guid tournamentId, CancellationToken cancellationToken);
        Task AddAsync(Domain.TournamentParticipant.TournamentParticipant tournamentParticipant, CancellationToken cancellationToken);
    }
}
