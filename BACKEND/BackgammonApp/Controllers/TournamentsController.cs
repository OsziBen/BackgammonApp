using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants.ApiRouteConstants;

namespace WebAPI.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route(TournamentConstants.Base)]
    public class TournamentsController : ControllerBase
    {
    }
}
