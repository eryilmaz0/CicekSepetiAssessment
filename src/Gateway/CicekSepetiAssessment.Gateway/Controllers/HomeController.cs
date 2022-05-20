using Microsoft.AspNetCore.Mvc;

namespace CicekSepetiAssessment.Gateway.Controllers;

[ApiController]
[Route("api/Home")]
public class HomeController : ControllerBase
{
   [HttpGet]
   [Route("HealthCheck")]
   public IActionResult HealthCheck()
   {
      return Ok("Gateway Health Check Success.");
   }
}