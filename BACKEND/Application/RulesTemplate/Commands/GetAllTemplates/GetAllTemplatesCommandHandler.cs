using Application.Interfaces.Repository.RulesTemplate;
using Application.RulesTemplate.Responses;
using MediatR;

namespace Application.RulesTemplate.Commands.GetAllTemplates
{
    public class GetAllTemplatesCommandHandler : IRequestHandler<GetAllTemplatesCommand, List<RulesTemplateResponse>>
    {
        private readonly IRulesTemplateReadRepository _rulesTemplateReadRepository;

        public GetAllTemplatesCommandHandler(IRulesTemplateReadRepository rulesTemplateReadRepository)
        {
            _rulesTemplateReadRepository = rulesTemplateReadRepository;
        }

        public async Task<List<RulesTemplateResponse>> Handle(GetAllTemplatesCommand request, CancellationToken cancellationToken)
        {
            var templates = await _rulesTemplateReadRepository.GetAllActiveAsync(cancellationToken);

            return templates.Select(t => new RulesTemplateResponse
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                AuthorName = t.Author?.UserName ?? "System",
                TargetScore = t.TargetScore,
                UseClock = t.UseClock,
                MatchTimePerPlayerInSeconds = t.MatchTimePerPlayerInSeconds,
                StartOfTurnDelayPerPlayerInSeconds = t.StartOfTurnDelayPerPlayerInSeconds,
                CrawfordRuleEnabled = t.CrawfordRuleEnabled,
                CreatedAt = t.CreatedAt,
            }).ToList();
        }
    }
}
