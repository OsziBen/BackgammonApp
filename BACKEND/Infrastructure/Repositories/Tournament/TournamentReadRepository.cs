using Application.Interfaces.Repository.Tournament;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Tournament
{
    public class TournamentReadRepository
        : ReadRepositoryBase<Domain.Tournament.Tournament>,
        ITournamentReadRepository
    {
        public TournamentReadRepository(ApplicationDbContext context) : base(context) { }
    }
}
