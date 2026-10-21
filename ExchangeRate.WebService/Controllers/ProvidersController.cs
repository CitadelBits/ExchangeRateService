using ExchangeRate.WebService.Dtos;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CurrencyAggregator.Service.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class ProvidersController : ControllerBase
  {
    private readonly ILogger<ProvidersController> _logger;
    private IProviderResolver _providerResolver;
    private readonly IOptions<ExchangeRatesOptions> _options;


    public ProvidersController(ILogger<ProvidersController> logger, 
      IProviderResolver providerResolver, 
      IOptions<ExchangeRatesOptions> options
      )
    {
      _logger = logger;
      _providerResolver = providerResolver;
      _options = options;
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> GetProviders()
    {
      var providers = _providerResolver.GetAll()
      .Select(providers => new ProviderDto(providers.Code,providers.DisplayName))
        .ToList();

      var response = new ProvidersResponseDto(
          _options.Value.DefaultProvider,
          providers);

      return Task.FromResult<IActionResult>(Ok(response));
    }

    [HttpGet()]
    [Route("{provider}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> GetCurrencies(string provider)
    {



      return Task.FromResult<IActionResult>(Ok());
    }

  }
}
