using Application.Interfaces.Repository.RulesTemplate;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RulesTemplate
{
    public class RulesTemplateReadRepository
        : ReadRepositoryBase<Domain.RulesTemplate.RulesTemplate>,
        IRulesTemplateReadRepository
    {
        public RulesTemplateReadRepository(ApplicationDbContext context) : base(context) { }

        public Task<List<Domain.RulesTemplate.RulesTemplate>> GetAllActiveAsync(CancellationToken cancellationToken)
            => Query()
                .Where(rt => rt.IsPublic)
                .Include(rt => rt.Author)
                .ToListAsync(cancellationToken);
    }
}
