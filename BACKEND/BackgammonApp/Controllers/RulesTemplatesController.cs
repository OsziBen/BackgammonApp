using Application.Interfaces.Common;
using Application.RulesTemplate.Commands.GetAllTemplates;
using Application.RulesTemplate.Responses;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants.ApiRouteConstants;

namespace WebAPI.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route(RulesTemplateConstants.Base)]
    public class RulesTemplatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;

        public RulesTemplatesController(
            IMediator mediator,
            ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<ActionResult<List<RulesTemplateResponse>>> ListAllTemplatesAsync(
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new GetAllTemplatesCommand();

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
