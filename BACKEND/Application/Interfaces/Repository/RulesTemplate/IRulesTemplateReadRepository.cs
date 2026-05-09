namespace Application.Interfaces.Repository.RulesTemplate
{
    public interface IRulesTemplateReadRepository
    {
        Task<List<Domain.RulesTemplate.RulesTemplate>> GetAllActiveAsync(CancellationToken cancellationToken);
    }
}
