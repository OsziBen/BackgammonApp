using Application.Interfaces.Repository.Tournament;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Tournament
{
    public class TournamentWriteRepository : ITournamentWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public TournamentWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
