using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/ping")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public ActionResult Ping() => Ok("pong");
    }
}
