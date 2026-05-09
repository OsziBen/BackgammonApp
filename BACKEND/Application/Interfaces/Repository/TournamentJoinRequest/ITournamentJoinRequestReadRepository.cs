namespace Application.Interfaces.Repository.TournamentJoinRequest
{
    public interface ITournamentJoinRequestReadRepository
    {
        Task<List<Domain.TournamentJoinRequest.TournamentJoinRequest>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Domain.TournamentJoinRequest.TournamentJoinRequest>> GetAllByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken);
        Task<bool> HasPendingRequestAsync(Guid userId, Guid tournamentId, CancellationToken cancellationToken);
    }
}
