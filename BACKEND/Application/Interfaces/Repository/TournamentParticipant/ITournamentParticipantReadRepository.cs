namespace Application.Interfaces.Repository.TournamentParticipant
{
    public interface ITournamentParticipantReadRepository
    {
        Task<bool> ExistsAsync(Guid userId, Guid tournamentId, CancellationToken cancellationToken);
        Task<List<Domain.TournamentParticipant.TournamentParticipant>> GetUsersByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken);
        Task<List<Guid>> GetParticipantTournamentIdsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
