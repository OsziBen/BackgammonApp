using Application.RulesTemplate.Responses;
using MediatR;

namespace Application.RulesTemplate.Commands.GetAllTemplates
{
    public record GetAllTemplatesCommand() : IRequest<List<RulesTemplateResponse>>;
}
