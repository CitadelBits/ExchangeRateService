using Microsoft.AspNetCore.Mvc;

namespace CurrencyAggregator.Service.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class ProvidersController : ControllerBase
  {
    private readonly ILogger<ProvidersController> _logger;

    public ProvidersController(ILogger<ProvidersController> logger)
    {
      _logger = logger;
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> GetProviders()
    {
      return Task.FromResult<IActionResult>(Ok("Empty"));
    }
  }
}
